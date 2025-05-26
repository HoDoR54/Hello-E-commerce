using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddNewUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);
    }
}
