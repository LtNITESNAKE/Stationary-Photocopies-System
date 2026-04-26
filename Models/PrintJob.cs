#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class PrintJob
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual User? Student { get; set; }

        public int? NoteId { get; set; }

        [ForeignKey("NoteId")]
        public virtual Note? Note { get; set; }

        public string? DocumentName { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Copies { get; set; } = 1;

        [Required]
        public string Status { get; set; } = "Queued";

        [Required]
        public DateTime EstimatedPickupTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
