using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;

namespace E_commerce_Admin_Dashboard.Interfaces.Services
{
    public interface IAdminService
    {
        Task<ServiceResult<List<AdminResponse>>> GetAllAdmins (string token, string? search, int limit, int page, string? sort);
    }
}
