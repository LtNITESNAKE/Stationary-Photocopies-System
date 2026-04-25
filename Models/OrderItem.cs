using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class OrderItem
    {
        // TODO for Malaika (Student Stationery Checkout):
        // This links specific Stationery Products (Pens, Paper) to a Student's Order.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add [ForeignKey("Order")] above OrderId.
        // 3. Add [ForeignKey("Product")] above ProductId.
        // 4. Add Quantity and UnitPrice.

        // EXAMPLE PROPERTIES:
        [Key]
        public int Id { get; set; }

       
    }
}
