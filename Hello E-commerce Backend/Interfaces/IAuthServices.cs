using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IAuthServices
    {
        Task<AdminLoginResponse> AdminLoginAsync(AdminLoginRequest req);
    }
}
