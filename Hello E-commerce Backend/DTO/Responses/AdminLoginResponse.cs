namespace E_commerce_Admin_Dashboard.DTO.Responses
{
    public class AdminLoginResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsSuperAdmin { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
