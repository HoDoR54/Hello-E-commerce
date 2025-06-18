using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public enum PaymentMethod
    {
        KbzPay,
        UabPay,
        CbPay,
        AyaPay,
        WavePay,
        Cash
    }

    public enum PaymentOption
    {
        Full,
        Installment
    }

    public enum PurchaseStatus
    {
        Pending,
        Completed,
        Cancelled
    }

    public class Purchase
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentOption PaymentOption { get; set; }

        [ForeignKey("ShippingAddress")]
        public Guid ShippingAddressId { get; set; }

        public CustomerAddress CustomerAddress { get; set; }

        public PurchaseStatus Status { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
