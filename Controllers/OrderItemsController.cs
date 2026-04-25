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
                /*.Include(o => o.Order)*/
                /*.Include(o => o.Product)*/
                .ToList();
            return View(orderItems);
        }

        // 2. AddToCart(int productId, int quantity) - POST
        // TODO: Find the Student's active 'Cart' Order. Create an OrderItem linking the Product to the Order.

        // 3. RemoveFromCart(int id) - POST
        // TODO: Delete the OrderItem from the database.
    }
}
