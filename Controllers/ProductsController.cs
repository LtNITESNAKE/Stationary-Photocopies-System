using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Misbah: You are managing the Stationery Items (Products). 
        // The Photocopier Operator uses your screens to add new pens, notebooks, etc. to sell to Students.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Fetches all Products from the database and passes them to the View.
        public IActionResult Index()
        {
            // TODO: Once you add the Category property to Product.cs, uncomment the .Include() below!
            var products = _context.Products/*.Include(p => p.Category)*/.ToList();
            return View(products);
        }

        // 2. Create() - GET
        // TODO: Return the empty view with a ViewBag of Categories so they can select a category from a dropdown.

        // 3. Create(Product product) - POST
        // TODO: Add the new Product to the database.
    }
}
