using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IAuthServices
    {
        Task<ServiceResult<AdminLoginResponse>> AdminLoginAsync(LoginRequest req);
        Task<ServiceResult<CustomerLoginResponse>> CustomerLoginAsync(LoginRequest req);
        Task<ServiceResult<CustomerRegisterResponse>> CustomerRegisterAsync(CustomerRegisterRequest req);
        bool VerifyPassword(string password, string hashed);
    }
}
