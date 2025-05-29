using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Mappers
{
    public interface IAdminMapper
    {
        AdminLoginResponse ToAdminLoginResponse(User user, Admin admin);
    }
}
