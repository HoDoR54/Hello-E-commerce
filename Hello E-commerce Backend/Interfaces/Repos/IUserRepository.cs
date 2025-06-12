using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Repos
{
    public interface IUserRepository
    {
        Task<User> AddNewUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);

        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByIdAsync(Guid id);

        Task<string?> UpdateEmailAsync(Guid id, string email);
    }
}
