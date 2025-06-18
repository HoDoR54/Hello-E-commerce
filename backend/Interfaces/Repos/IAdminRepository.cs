using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Repos
{
    public interface IAdminRepository
    {
        Task<Admin?> GetAdminByUserIdAsync(Guid userId);

        Task<List<Admin>> GetAllAdminsAsync(string? search, int limit, int page, string? sort);

        Task<Admin> AddNewAdminAsync (Admin admin);

        Task<Admin?> GetAdminByIdAsync (Guid id);
        Task<string?> UpdatePhoneNumAsync(Guid id, string phoneNumber);
        Task<string?> UpdateNameAsync(Guid id, string name);
        
        Task<Admin?> DeleteAdminByIdAsync(Guid id);
        Task<Admin?> PromoteAdminAsync(Guid id);
        Task<Admin?> DemoteAdminAsync(Guid id);
    }
}
