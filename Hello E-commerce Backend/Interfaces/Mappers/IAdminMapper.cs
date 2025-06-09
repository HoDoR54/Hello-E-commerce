using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Mappers
{
    public interface IAdminMapper
    {
        AdminResponse ToAdminLoginResponse(User user, Admin admin);
        User CreateAdminRequestToUserModel (CreateAdminRequest request);

        Admin CreateAdminRequestToAdminModel (CreateAdminRequest request, User user, Admin superAdmin);
    }
}
