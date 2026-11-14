using System.ComponentModel.DataAnnotations;
namespace StockFlow.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; }
            = DateTime.UtcNow;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
