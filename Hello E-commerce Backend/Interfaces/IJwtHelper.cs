using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface IJwtHelper
    {
        string GetSecretKey();
        string GenerateToken(User user, TokenType tokenType);
    }
}
