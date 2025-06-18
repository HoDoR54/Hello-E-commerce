using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public enum CustomerActionType
    {
        View,
        Rate,
        Refund
    }

    public enum RefundStatus
    {
        Requested,
        Approved,
        Rejected
    }
    public abstract class CustomerAction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public CustomerActionType ActionType { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class View : CustomerAction
    {
    }

    public class Rate : CustomerAction
    {
        public int Rating { get; set; }
    }

    public class Refund : CustomerAction
    {
        [ForeignKey("Purchase")]
        public Guid PurchaseId { get; set; }

        public string? Reason { get; set; }

        public RefundStatus Status { get; set; }
    }
}
