using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Models;

public interface IAuthServices
{
    Task<ServiceResult<AdminLoginResponse>> LoginAsAdminAsync(LoginRequest request);
    Task<ServiceResult<CustomerLoginResponse>> LoginAsCustomerAsync(LoginRequest request);
    Task<ServiceResult<CustomerRegisterResponse>> RegisterCustomerAsync(CustomerRegisterRequest request);

    Task<ServiceResult<TokenPair>> GenerateTokenPairAsync(string email);
    Task<ServiceResult<string>> GenerateTokenAsync(string? email, TokenType type, string? refreshToken);

    Task<ServiceResult<string>> ValidateTokenAsync(string token, TokenType type);
    bool IsPasswordValid(string inputPassword, string hashedPassword);

    Task<ServiceResult<User>> GetUserByEmailAsync(string email);
    Task<ServiceResult<string>> RefreshAccessToken(string refreshToken);
}
