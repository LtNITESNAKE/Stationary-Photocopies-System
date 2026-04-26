using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;
using System.Security.Claims;

namespace PhotocopySystem.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Index() - GET: Lists all items in all orders
        public IActionResult Index()
        {
            var orderItems = _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .ToList();
            return View(orderItems);
        }

        // 2. RemoveFromCart(int id) - POST: Delete an OrderItem
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
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