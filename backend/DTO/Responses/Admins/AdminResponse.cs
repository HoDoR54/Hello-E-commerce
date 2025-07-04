using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.DTO.Responses.Admins
{
    public class AdminResponse
    {
        public UserResponse User { get; set; }

        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public bool IsSuperAdmin { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
