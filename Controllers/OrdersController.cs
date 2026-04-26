using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace PhotocopySystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.IsInRole("Admin");

            var query = _context.Orders.Include(o => o.User).AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(o => o.UserId == userId);
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 2. Buy Product (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var product = _context.Products.Find(productId);
            var stock = _context.InventoryStocks.FirstOrDefault(s => s.ProductId == productId);

            if (product == null || stock == null || stock.QuantityAvailable < quantity)
            {
                TempData["Error"] = "Insufficient stock or invalid product.";
                return RedirectToAction("Index", "InventoryStocks");
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Create the Order
                        var order = new Order
                        {
                            UserId = userId,
                            OrderDate = DateTime.Now,
                            TotalAmount = product.Price * quantity
                        };
                        _context.Orders.Add(order);
                        _context.SaveChanges();

                        // Create the Order Item
                        var item = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = productId,
                            Quantity = quantity,
                            UnitPrice = product.Price
                        };
                        _context.OrderItems.Add(item);
                        _context.SaveChanges(); 
                        // NOTE: The database trigger 'trg_AutoDeductStock' will automatically update the stock now!

                        transaction.Commit();
                        TempData["Success"] = $"Successfully purchased {quantity} {product.Name}(s)!";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TempData["Error"] = "Purchase failed: " + ex.Message;
                    }
                }
            });

            return RedirectToAction("Index");
        }
    }
}