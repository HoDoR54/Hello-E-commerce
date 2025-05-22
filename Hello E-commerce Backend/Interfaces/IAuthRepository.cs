using E_commerce_Admin_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IAuthRepository
    {
        Task<Admin?> GetAdminByEmailAsync(string email);

        Task<User?> GetUserByEmailAsync(string email);

        Task<Customer?> GetCustomerByEmailAsync(string email);
    }
}
