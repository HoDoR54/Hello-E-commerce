using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public static class AdminMappers
    {
        public static AdminLoginResponse ToAdminLoginResponse(User user, Admin admin)
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
