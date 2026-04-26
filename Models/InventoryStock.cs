using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopySystem.Models
{
    public class InventoryStock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public int QuantityAvailable { get; set; } = 0;

        [Timestamp]
        public byte[] RowVersion { get; set; } // Matches SQL ROWVERSION
    }
}
