using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PhotocopySystem.Controllers
{
    [Authorize]
    public class InventoryStocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryStocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Shop Floor (Live Stock Fetch) — any logged-in user
        public IActionResult Index()
        {
            var stock = _context.InventoryStocks
                .Include(s => s.Product)
                .ThenInclude(p => p.Category)
                .ToList();
            return View(stock);
        }

        // 2. Edit Stock (Admin only) — Bug 6 fix
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var stock = _context.InventoryStocks.Include(s => s.Product).FirstOrDefault(s => s.Id == id);
            return View(stock);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(InventoryStock stock)
        {
            var dbStock = _context.InventoryStocks.Find(stock.Id);
            if (dbStock != null)
            {
                dbStock.QuantityAvailable = stock.QuantityAvailable;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
