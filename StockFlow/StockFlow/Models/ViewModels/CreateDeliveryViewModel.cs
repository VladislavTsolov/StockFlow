namespace StockFlow.Models.ViewModels
{
    public class CreateDeliveryViewModel
    {
        public DateTime DeliveryDate { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<DeliveryProductViewModel> Products { get; set; } = new();
    }

    public class DeliveryProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}