using E_commerce_Admin_Dashboard.DTO.Responses.Auth;

namespace E_commerce_Admin_Dashboard.DTO.Responses.Customers
{
    public class CustomerResponse
    {
        // from User.cs
        public UserResponse User { get; set; }

        // from Customer.cs
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int LoyaltyPoints { get; set; }
    }
}
