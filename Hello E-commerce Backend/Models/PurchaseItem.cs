using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public class PurchaseItem
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Purchase")]
        public Guid PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
