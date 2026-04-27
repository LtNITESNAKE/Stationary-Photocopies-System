using System;
using System.ComponentModel.DataAnnotations;

namespace PhotocopySystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Teacher, Student

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal Balance { get; set; } = 0.00m;
    }
}
