using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class InventoryStocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryStocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Noor (CONCURRENCY TASK!): You are managing the Live Stationery Inventory!
        // When multiple Students try to buy the last remaining Pen at the EXACT same time, you must handle the DbUpdateConcurrencyException!

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows a list of all products and their CURRENT QuantityAvailable.
        public IActionResult Index()
        {
            var stocks = _context.InventoryStocks.Include(i => i.Product).ToList();
            return View(stocks);
        }

        // 2. Edit(int id) - GET
        // TODO: Fetch the specific InventoryStock row and return it to the View.

        // 3. Edit(InventoryStock stock) - POST
        // TODO: This is where you catch DbUpdateConcurrencyException! Try saving _context.SaveChanges(). 
        // If it throws the exception, it means a Student bought the item while the Operator was updating it.
    }
}
