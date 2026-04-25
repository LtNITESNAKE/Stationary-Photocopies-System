using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Misbah: You are managing the Stationery Categories (e.g., Pens, Paper, Binding).
        // The Operator creates these categories to organize the products.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows all categories.
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // 2. Create() - GET
        // TODO: Just return View();

        // 3. Create(Category category) - POST
        // TODO: Save the category to the database.
    }
}
