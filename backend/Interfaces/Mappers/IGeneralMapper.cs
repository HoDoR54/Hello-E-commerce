using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Mappers
{
    public interface IGeneralMapper
    {
        RefreshToken RefreshTokenStringToModel(string refreshToken, Guid userId);
    }
}
