using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public class GeneralMapper : IGeneralMapper
    {
        public RefreshToken RefreshTokenStringToModel (string  refreshToken, Guid userId)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                UserId = userId
            };
        }

        public UserResponse UserModelToResponse(User userModel)
        {
            return new UserResponse
            {
                Id = userModel.Id,
                Email = userModel.Email,
                Role = userModel.Role,
                IsWarned = userModel.IsWarned,
                WarningLevel = userModel.WarningLevel,
                IsBanned = userModel.IsBanned,
                BannedDays = userModel.BannedDays,
                CreatedAt = userModel.CreatedAt,
                UpdatedAt = userModel.UpdatedAt,
            };
        }
    }
}
