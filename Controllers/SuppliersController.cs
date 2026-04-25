using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Muskan: You are managing the external Suppliers.
        // The Photocopier Operator buys raw paper and pens from these suppliers to stock the shop.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Fetches all Suppliers from the database and displays them.
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();
            return View(suppliers);
        }

        // 2. Create() - GET
        // TODO: Show the form to add a new Supplier.

        // 3. Create(Supplier supplier) - POST
        // TODO: Save the supplier to the DB.
    }
}
