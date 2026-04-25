using System.ComponentModel.DataAnnotations;

namespace PhotocopySystem.Models
{
    public class Supplier
    {
        // TODO for Muskan (Photocopier Operator - Suppliers):
        // You are managing external Suppliers. The Operator buys raw paper/ink from them.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add CompanyName, ContactPerson, Email, Phone. Check DatabaseScript.sql.

        // EXAMPLE PROPERTIES:
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        // Your turn! Add ContactPerson, Email, Phone
    }
}
