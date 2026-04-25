using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class InventoryStock
    {
        // TODO for Noor (Concurrency Task - Live Inventory):
        // You are managing the live stock levels of Stationery.
        // When multiple Students purchase items simultaneously, your concurrency logic kicks in!
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add [ForeignKey("Product")] above ProductId.
        // 3. Ensure QuantityAvailable tracks the stock.
        // 4. CRITICAL: Add a [Timestamp] byte[] RowVersion property. This tells Entity Framework to throw a DbUpdateConcurrencyException if two students buy at the exact same time!    
        
        // EXAMPLE PROPERTIES:
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // Now your turn! Add 'QuantityAvailable' (int)
         public int QuantityAvailable { get; set; }
        
        // IMPORTANT FOR CONCURRENCY:
         [Timestamp]
         public byte[] RowVersion { get; set; }
    }
}
