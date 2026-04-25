using System.ComponentModel.DataAnnotations;

namespace PhotocopySystem.Models
{
    public class ServerNode
    {
        // TODO for Awais/Hadain:
        // This represents a simulated printer/server in our Load Balancer.
        // Check DatabaseScript.sql for columns.
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        // TODO for Awais & Hadain (Physical Printers/Server Nodes):
        // These represent the physical Photocopy machines processing student requests.
        // The load balancer assigns PrintJobs to the node with the lowest CurrentActiveJobs.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add ServerName, IPAddress.
        // 3. Add IsOnline (boolean) and CurrentActiveJobs (int).
        public string ServerName { get; set; }

        public string IPAddress { get; set; }

        public bool IsOnline { get; set; }

        public int CurrentActiveJobs { get; set; }
    }
}
