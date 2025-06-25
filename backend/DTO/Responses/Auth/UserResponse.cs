using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.DTO.Responses.Auth
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public bool IsWarned { get; set; }
        public int? WarningLevel { get; set; }

        public bool IsBanned { get; set; }
        public int? BannedDays { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
