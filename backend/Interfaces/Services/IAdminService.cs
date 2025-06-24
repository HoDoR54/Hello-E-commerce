using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;

namespace E_commerce_Admin_Dashboard.Interfaces.Services
{
    public interface IAdminService
    {
        Task<ServiceResult<AdminResponse>> CreateNewAdminAsync(CreateAdminRequest req);
        Task<ServiceResult<AdminResponse>> DeleteAdminByIdAsync(Guid id);
        Task<ServiceResult<AdminResponse>> DemoteAdminAsync(Guid id);
        Task<ServiceResult<AdminResponse>> GetAdminByIdAsync(Guid id);
        Task<ServiceResult<List<AdminResponse>>> GetAllAdminsAsync(string? search, int limit, int page, string? sort);
        Task<ServiceResult<AdminResponse>> PromoteAdminAsync(Guid id);
        Task<ServiceResult<AdminResponse>> UpdateAdminDetailsAsync(Guid id, UpdateAdminDetailsRequest req);
    }
}
