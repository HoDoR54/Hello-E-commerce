using E_commerce_Admin_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IAuthRepository
    {
        Task<Admin?> GetAdminByUserIdAsync(Guid Id);

        Task<User?> GetUserByEmailAsync(string email);

        Task<Customer?> GetCustomerByUserIdAsync(Guid Id);
    }
}
