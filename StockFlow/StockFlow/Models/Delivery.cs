using System.ComponentModel.DataAnnotations;

namespace StockFlow.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
