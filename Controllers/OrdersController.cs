using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Malaika: You are managing Student Stationery Purchases!
        // While students are printing notes, they can also buy pens, notebooks, etc.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Displays a list of all Orders. Uses .Include() to show the Student's name.
        public IActionResult Index()
        {
            // TODO: Once you finish the Order Model, uncomment the .Include() line!
            var orders = _context.Orders
                /*.Include(o => o.User)*/ // The Student
                .ToList();
            return View(orders);
        }

        // 2. Details(int id) - GET
        // TODO: Fetch the order and .Include(o => o.OrderItems).ThenInclude(i => i.Product) to show what they bought.
    }
}
