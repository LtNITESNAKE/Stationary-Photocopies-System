using System;
using System.ComponentModel.DataAnnotations; // IMPORTANT: You need this to use [Key] and [Required]

namespace PhotocopySystem.Models
{
    /// <summary>
    /// GOLDEN EXAMPLE (By Mujtaba)
    /// This is the full User model. Everyone else should look at this to see how to build their own models!
    /// </summary>

    public class User
    {
        // 1. [Key] tells Entity Framework that this is the Primary Key in the database.
        [Key]
        public int Id { get; set; }

        // 2. [Required] means this cannot be NULL in the database.
        // [StringLength] sets the max size, like NVARCHAR(100) in SQL.
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        // 3. [EmailAddress] adds automatic validation to make sure they type a real email in the UI.
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        // 4. We can set a default value in C# just like DEFAULT GETDATE() in SQL.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
