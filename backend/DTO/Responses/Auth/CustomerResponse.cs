namespace E_commerce_Admin_Dashboard.DTO.Responses.Auth
{
    public class CustomerResponse
    {
        // from User.cs
        public Guid UserId { get; set; }
        public string Email { get; set; }

        // from Customer.cs
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsWarned { get; set; }
        public int? WarningLevel { get; set; }

        public bool IsBanned { get; set; }
        public int? BannedDays { get; set; }

        public int LoyaltyPoints { get; set; }
    }
}
