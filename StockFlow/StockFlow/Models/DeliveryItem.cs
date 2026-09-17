using System.ComponentModel.DataAnnotations;

namespace StockFlow.Models
{
    public class DeliveryItem
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityDelivered { get; set; }
        public Delivery Delivery { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
