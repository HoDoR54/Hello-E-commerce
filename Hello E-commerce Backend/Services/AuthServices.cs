using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Mappers;

public class AuthServices : IAuthServices
{
    private readonly IAuthRepository _authRepo;

    public AuthServices(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<AdminLoginResponse?> AdminLoginAsync(LoginRequest req)
    {
        var matchedUser = await _authRepo.GetUserByEmailAsync(req.Email);
        if (matchedUser == null) return null;

        var matchedAdmin = await _authRepo.GetAdminByEmailAsync(req.Email);
        if (matchedAdmin == null) return null;

        string? hashed = matchedUser.Password;
        if (hashed == null) return null;

        if (!VerifyPassword(req.Password, hashed)) return null;

        return AdminMappers.ToAdminLoginResponse(matchedUser, matchedAdmin);
    }

    public async Task<CustomerLoginResponse?> CustomerLoginAsync(LoginRequest req)
    {
        var matchedUser = await _authRepo.GetUserByEmailAsync(req.Email);
        if (matchedUser == null) return null;

        var matchedCustomer = await _authRepo.GetCustomerByEmailAsync(req.Email);
        if (matchedCustomer == null) return null;

        string hashed = matchedUser.Password;
        if (hashed == null) return null;

        if (VerifyPassword(req.Password, hashed)) return null;

        return CustomerMappers.ToCustomerLoginResponse(matchedUser, matchedCustomer);
    }

    private bool VerifyPassword(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}
