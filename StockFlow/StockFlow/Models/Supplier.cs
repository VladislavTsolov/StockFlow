using System.ComponentModel.DataAnnotations;
namespace StockFlow.Models

{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
            = new List<Delivery>();
    }
}
