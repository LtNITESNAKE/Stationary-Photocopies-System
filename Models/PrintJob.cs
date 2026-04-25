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

        // The Student who requested the print
   

        // The Note they are printing (nullable if they just uploaded a custom document)
   
    
        
        // Queued, Printing, Completed, Cancelled
        public string Status { get; set; } = "Queued";

        public int? AssignedServerId { get; set; }
        [ForeignKey("AssignedServerId")]
        public virtual ServerNode ServerNode { get; set; }

    }
}
