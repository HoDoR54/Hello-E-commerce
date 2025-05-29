using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public class AdminMapper : IAdminMapper
    {
        public AdminLoginResponse ToAdminLoginResponse(User user, Admin admin)
        {
            return new AdminLoginResponse
            {
                UserId = user.Id,
                Email = user.Email,

                AdminId = admin.Id,
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                IsSuperAdmin = admin.IsSuperAdmin,
                CreatedAt = admin.CreatedAt
            };
        }
    }
}
