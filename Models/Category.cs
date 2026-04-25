using System.ComponentModel.DataAnnotations; // <-- Always add this!

namespace PhotocopySystem.Models
{
    public class Category
    {
        // TODO for Misbah (Photocopier Operator - Stationery Categories):
        // As the Operator, you need to categorize stationery (e.g., Pens, Notebooks).
        // Instructions:
        // 1. Add [Key] above Id to make it the Primary Key.
        // 2. Add [Required] and [StringLength(100)] above Name so it can't be empty.
        // 3. Add [StringLength(500)] above Description..sql to see the columns for Category (Id, Name, Description).
        
        // EXAMPLE PROPERTY:
        [Key] // This makes 'Id' the Primary Key
        public int Id { get; set; }

        // Now it is your turn! Add 'Name' and 'Description' below.
        // Make sure 'Name' is [Required]!
    }
}
