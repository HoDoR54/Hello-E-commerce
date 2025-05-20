using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public class CustomerAddressDetail
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
    }
}
