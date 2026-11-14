using System.ComponentModel.DataAnnotations;
namespace StockFlow.Models

{
    public class Delivery
    {
        public int Id { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        [StringLength(500)]
        public string? Notes { get; set; }

        public ICollection<DeliveryItem> DeliveryItems { get; set; }
            = new List<DeliveryItem>();
    }
}
