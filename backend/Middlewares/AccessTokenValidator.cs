using System.Security.Claims;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.AspNetCore.Http;

namespace E_commerce_Admin_Dashboard.Middlewares
{
    public class AccessTokenValidator : IMiddleware
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepo;
        private readonly IAdminRepository _adminRepo;

        public AccessTokenValidator(IJwtHelper jwtHelper, IUserRepository userRepo, IAdminRepository adminRepo)
        {
            _jwtHelper = jwtHelper;
            _userRepo = userRepo;
            _adminRepo = adminRepo;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = context.Request.Path.Value;

            if (path != null && (
                path.StartsWith("/api/auth", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)))
            {
                await next(context);
                return;
            }

            var accessToken = context.Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(accessToken))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing access token.");
                return;
            }

            var validationResult = await _jwtHelper.ValidateTokenAsync(accessToken, TokenType.Access);
            if (!validationResult.OK)
            {
                context.Response.StatusCode = validationResult.StatusCode;
                await context.Response.WriteAsync(validationResult.ErrorMessage);
                return;
            }

            var userId = _jwtHelper.GetUserIdByToken(validationResult.Data.ToString());

            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("User not found.");
                return;
            }

            string role;
            if (user.Role == UserRole.Customer)
            {
                role = "customer";
            }
            else
            {
                var admin = await _adminRepo.GetAdminByUserIdAsync(userId);
                if (admin == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Admin not found.");
                    return;
                }
                role = admin.IsSuperAdmin ? "super_admin" : "admin";
            }

            var claims = new List<Claim>
            {
                new Claim("user_id", userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "Custom");
            context.User = new ClaimsPrincipal(identity);

            await next(context);
        }
    }
}
