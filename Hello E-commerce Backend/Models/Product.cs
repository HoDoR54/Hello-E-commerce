using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PriceInMMK { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        [Range(0, int.MaxValue)]
        public int InStockQuantity { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CustomerAction> CustomerActions { get; set; }

        public ICollection<Favorite> Favorites { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
