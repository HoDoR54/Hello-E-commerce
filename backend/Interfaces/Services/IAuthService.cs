using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Models;

public interface IAuthService
{
    Task<ServiceResult<UserResponse>> LoginAsync(LoginRequest request);
    Task<ServiceResult<CustomerResponse>> RegisterCustomerAsync(CustomerRegisterRequest request);
    Task<ServiceResult<TokenPair>> GenerateTokenPairAsync(string email);
    Task<ServiceResult<string>> GenerateTokenAsync(string? email, TokenType type);
    Task<ServiceResult<string>> ValidateTokenAsync(string token, TokenType type);
    Task<ServiceResult<User>> GetUserByEmailAsync(string email);

    Task<ServiceResult<RefreshToken>> AddNewRefreshTokenAsync(string refreshToken);
    Task<ServiceResult<string>> RefreshAccessToken(string refreshToken);

    Task<ServiceResult<UserResponse>> GetUserByTokenAsync(string token);

}
