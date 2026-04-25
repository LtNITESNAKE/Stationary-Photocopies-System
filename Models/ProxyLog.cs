using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class ProxyLog
    {
        // TODO for Awais (Network Traffic Monitor):
        // This tracks the network traffic of Students sending Print Jobs to physical ServerNodes.
        // Instructions:
        // 1. Add [Key] to Id.
        // 2. Add IncomingIp, RequestType, Timestamp.
        // 3. Add [ForeignKey("ServerNode")] above RoutedToServerId.

        // EXAMPLE PROPERTIES:
        [Key]
        public int Id { get; set; }

        public string IncomingIp { get; set; }
        public string RequestType { get; set; }

        [ForeignKey("ServerNode")]
        public int? RoutedToServerId { get; set; }
        public virtual ServerNode ServerNode { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
