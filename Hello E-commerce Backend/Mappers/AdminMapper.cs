using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public class AdminMapper : IAdminMapper
    {

        public AdminResponse ToAdminLoginResponse(User user, Admin admin)
        {
            return new AdminResponse
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
