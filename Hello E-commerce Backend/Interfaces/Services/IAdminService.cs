using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;

namespace E_commerce_Admin_Dashboard.Interfaces.Services
{
    public interface IAdminService
    {
        Task<ServiceResult<List<AdminResponse>>> GetAllAdminsAsync (string token, string? search, int limit, int page, string? sort);

        Task<ServiceResult<AdminResponse>> CreateNewAdminAsync (string token, CreateAdminRequest req);

        Task<ServiceResult<AdminResponse>> GetAdminByIdAsync (string token, Guid id);

        Task<ServiceResult<AdminResponse>> UpdateAdminDetailsAsync (Guid id, UpdateAdminDetailsRequest req);

        Task<ServiceResult<AdminResponse>> DeleteAdminByIdAsync(Guid id);

        Task<ServiceResult<AdminResponse>> PromoteAdminAsync(string token, Guid id);
        Task<ServiceResult<AdminResponse>> DemoteAdminAsync(string token, Guid id);
    }
}
