using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Malaika: You are managing the Student's Shopping Cart!
        // This is where Students add Stationery Products before they checkout.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Lists all items currently in all orders (for admin viewing).
        public IActionResult Index()
        {
            // TODO: Once you finish the OrderItem Model, uncomment these .Include lines!
            var orderItems = _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .ToList();
            return View(orderItems);
        }

        // 2. AddToCart(int productId, int quantity) - POST
        // TODO: Find the Student's active 'Cart' Order. Create an OrderItem linking the Product to the Order.
        public IActionResult AddToCart(int productId, int quantity)
        {
            // FIX: Get current logged-in user
            var email = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return RedirectToAction("Login", "Users");

            // FIX: Find the Student's active Cart, or CREATE ONE if they don't have it!
            var cartOrder = _context.Orders.FirstOrDefault(o => o.Status == "Cart" && o.UserId == user.Id);
            if (cartOrder == null)
            {
                cartOrder = new Order { UserId = user.Id, Status = "Cart", TotalAmount = 0 };
                _context.Orders.Add(cartOrder);
                _context.SaveChanges(); // Save to generate the Order ID
            }

            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var newItem = new OrderItem
                {
                    OrderId = cartOrder.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price // FIX: Uncommented Price!
                };

                _context.OrderItems.Add(newItem);
                
                // FIX: Update the parent Cart total!
                cartOrder.TotalAmount += (product.Price * quantity);
                
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // 3. RemoveFromCart(int id) - POST
        // TODO: Delete the OrderItem from the database.
        public IActionResult RemoveFromCart(int id)
        {
            // FIX: Include the parent Order so we can deduct the total!
            var item = _context.OrderItems.Include(i => i.Order).FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                if (item.Order != null)
                {
                    item.Order.TotalAmount -= (item.UnitPrice * item.Quantity);
                }
                
                _context.OrderItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}