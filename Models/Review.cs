using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class Review
    {
        // TODO for Fazil (Student Feedback System):
        // Allows Students to leave a review (Rating 1-5) on the Stationeries they bought.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add [ForeignKey("User")] above UserId (This is the Student).
        // 3. Add [ForeignKey("Product")] above ProductId.
        // 4. Add Rating (int) and Comment (string).
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        // Your turn! Add Rating (int), Comment (string), CreatedAt (DateTime)
    }
}
