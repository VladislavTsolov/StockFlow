using System.ComponentModel.DataAnnotations;

namespace StockFlow.Models.ViewModels
{
    public class CreateDeliveryViewModel
    {
        public DateTime DeliveryDate { get; set; }
        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Notes { get; set; }

        public List<DeliveryProductViewModel> Products { get; set; } = new();
    }

    public class DeliveryProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}