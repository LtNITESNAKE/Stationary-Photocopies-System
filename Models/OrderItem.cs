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
        //1:
        [Key]
        public int Id { get; set; }

        // 2. 
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        // 3.
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // 4.
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

    }
}
