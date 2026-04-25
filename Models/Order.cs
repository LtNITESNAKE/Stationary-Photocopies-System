using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class Order
    {
        // TODO for Malaika:
        // This is the main Order ticket. Check DatabaseScript.sql.

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        // TODO for Malaika (Student Stationery Checkout):
        // This is a Student's shopping cart or finalized purchase of stationery items.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add [ForeignKey("User")] above UserId (This is the Student!).
        // 3. Add OrderDate, TotalAmount, Status.
        // 4. Add an ICollection<OrderItem> OrderItems so one Order can contain multiple products!:
        // public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        //3.
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [StringLength(50)]
        public string Status { get; set; } = "Cart";

        //4.
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        
    }
}
