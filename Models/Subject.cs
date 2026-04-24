using System.ComponentModel.DataAnnotations;

namespace PhotocopySystem.Models
{
    public class Subject
    {
        // TODO for Misbah (or whoever is assigned!):
        // This holds the subjects that teachers teach.

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
