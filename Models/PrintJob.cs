using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class PrintJob
    {
        // TODO for Maryam:
        // This tracks the print requests. Check DatabaseScript.sql for columns. 
        // Add [Required] to DocumentName and Copies. 
        // Add [ForeignKey("Note")] above NoteId, and a virtual Note property.
        // Add [ForeignKey("ServerNode")] above AssignedServerId, and a virtual ServerNode property.
        // Add [Required] to Status and EstimatedPickupTime.
        // Add [ForeignKey("Student")] above StudentId, and a virtual Student property.
        // Add FineAmount (decimal) and a default value of 0. 


        // EXAMPLE PROPERTIES:

        [Key]
        public int Id { get; set; }

        [Required]
        public string DocumentName { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Copies { get; set; }

        // The Student who requested the print
        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        // The Note they are printing (nullable if they just uploaded a custom document)
        public int? NoteId { get; set; }

        [ForeignKey("NoteId")]
        public virtual Note Note { get; set; }

        // Queued, Printing, Completed, Cancelled
        [Required]
        public string Status { get; set; } = "Queued";

        [Required]
        public DateTime EstimatedPickupTime { get; set; }

        // Server assignment
        public int? AssignedServerId { get; set; }

        [ForeignKey("AssignedServerId")]
        public virtual ServerNode ServerNode { get; set; }

        // Financial 
        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0;

        // Tracking
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
