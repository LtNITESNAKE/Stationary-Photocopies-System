using System.ComponentModel.DataAnnotations; // <-- Always add this!
using System.ComponentModel.DataAnnotations.Schema; // Needed for Foreign Keys

namespace PhotocopySystem.Models
{
    public class Product
    {
        // TODO for Misbah (Photocopier Operator - Stationery Products):
        // You are managing the physical items sold to Students.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add [Required] and [StringLength(100)] to Name.
        // 3. Add [StringLength(500)] to Description.
        // 4. Add [Required] to Price.
        // 5. Add [ForeignKey("Category")] above CategoryId, and add a virtual Category property below it!' like this:
        // [ForeignKey("Category")]
        // public int CategoryId { get; set; }
        // public virtual Category Category { get; set; } // This is Navigation Property
        
        // EXAMPLE PROPERTIES:
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


    }
}
