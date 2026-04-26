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
         public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventoryStock = await _context.InventoryStocks
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inventoryStock == null) return NotFound();
            
            return View(inventoryStock);
        }

        // 3. Edit(InventoryStock stock) - POST
        // TODO: This is where you catch DbUpdateConcurrencyException! Try saving _context.SaveChanges(). 
        // If it throws the exception, it means a Student bought the item while the Operator was updating it.
         [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,QuantityAvailable,RowVersion")] InventoryStock stock)
{
    if (id != stock.Id) return NotFound();

    if (ModelState.IsValid)
    {
        try
        {
            _context.Entry(stock).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var databaseValues = await entry.GetDatabaseValuesAsync();

            if (databaseValues == null)
            {
                ModelState.AddModelError(string.Empty, 
                    "Unable to save. The item was deleted by another user.");
            }
            else
            {
                var dbValues = (InventoryStock)databaseValues.ToObject();

                if (dbValues.QuantityAvailable != stock.QuantityAvailable)
                {
                    ModelState.AddModelError("QuantityAvailable", 
                        $"Current database value: {dbValues.QuantityAvailable}");
                }

                ModelState.AddModelError(string.Empty,
                    "The record you attempted to edit was modified by another student. " +
                    "The current values in the database are displayed below. If you still want to edit, click Save again.");

                stock.RowVersion = dbValues.RowVersion;
                ModelState.Remove("RowVersion");
            }
        }
    }

    // FIX: Re-load the Product data so the View doesn't crash when trying to display @Model.Product.Name
    stock.Product = await _context.Products.FindAsync(stock.ProductId);
    
    return View(stock);
}
}
}
