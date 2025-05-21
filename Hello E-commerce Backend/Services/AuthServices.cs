using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class AuthServices : IAuthServices
{
    private readonly IAuthRepository _authRepo;

    public AuthServices(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<AdminLoginResponse?> AdminLoginAsync(AdminLoginRequest req)
    {
        var matchedAdmin = await _authRepo.GetAdminByEmailAsync(req.Email);
        if (matchedAdmin == null)
            return null;

        if (!VerifyPassword(req.Password, matchedAdmin.Password))
            return null;

        return AdminMappers.ModelToLoginResponse(matchedAdmin);
    }

    private bool VerifyPassword(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}
