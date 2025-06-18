using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public Cart Cart { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
