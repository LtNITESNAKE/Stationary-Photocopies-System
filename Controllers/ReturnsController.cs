using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PhotocopySystem.Controllers
{
    [Authorize]
    public class ReturnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReturnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Return Screen (GET)
        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Fetch products this user has bought
            var boughtItems = _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.UserId == userId)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    AvailableQuantity = g.Sum(oi => oi.Quantity),
                    Price = g.First().UnitPrice
                })
                .ToList();

            ViewBag.BoughtItems = boughtItems;

            var recentReturns = _context.ReturnRecords
                .Include(r => r.Product)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReturnDate)
                .ToList();

            return View(recentReturns);
        }

        // 2. Return Item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessReturn(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var student = _context.Users.Find(userId);
            var admin = _context.Users.FirstOrDefault(u => u.Role == "Admin");
            var product = _context.Products.Find(productId);
            var stock = _context.InventoryStocks.FirstOrDefault(s => s.ProductId == productId);

            if (student == null || product == null || stock == null || admin == null)
            {
                TempData["Error"] = "Invalid return request.";
                return RedirectToAction("Index");
            }

            // Check if user actually has this many items
            var totalBought = _context.OrderItems
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.UserId == userId && oi.ProductId == productId)
                .Sum(oi => oi.Quantity);

            if (quantity <= 0 || quantity > totalBought)
            {
                TempData["Error"] = "Invalid quantity to return.";
                return RedirectToAction("Index");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal refundAmount = product.Price * quantity;

                    // 1. Update Admin Stock
                    stock.QuantityAvailable += quantity;

                    // 2. Update Student Balance (Refund)
                    student.Balance += refundAmount;

                    // 3. Update Admin Balance (Deduct)
                    admin.Balance -= refundAmount;

                    // 4. Record the return in the new table
                    var returnRecord = new ReturnRecord
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        RefundAmount = refundAmount,
                        ReturnDate = DateTime.Now
                    };
                    _context.ReturnRecords.Add(returnRecord);
                    
                    _context.SaveChanges();
                    transaction.Commit();

                    TempData["Success"] = $"Successfully returned {quantity} {product.Name}(s). Rs. {refundAmount:F2} added to your balance.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Error"] = "Return failed: " + ex.Message;
                }
            }

            return RedirectToAction("Index");
        }
    }
}
