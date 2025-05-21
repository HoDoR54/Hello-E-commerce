using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public static class AdminMappers
    {
        public static AdminLoginResponse ModelToLoginResponse (Admin model)
        {
            return new AdminLoginResponse
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsSuperAdmin = model.IsSuperAdmin,
                CreatedAt = model.CreatedAt,
            };
        }
    }
}
