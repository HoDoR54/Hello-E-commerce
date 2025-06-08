using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Models;
using E_commerce_Admin_Dashboard.Repositories;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Schema;

namespace E_commerce_Admin_Dashboard.Helpers
{
    public enum TokenType
    {
        Access,
        Refresh
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
        private readonly IUserRepository _userRepo;
        private readonly IAdminRepository _adminRepo;
        public JwtHelper (IConfiguration configuration, IUserRepository userRepository, IAdminRepository adminRepo)
        {
            _configuration = configuration;
            _userRepo = userRepository;
            _adminRepo = adminRepo;
        }

        public string GetSecretKey ()
        {
            return _configuration["Jwt:Key"];
        }
        public string GenerateToken (User user, TokenType tokenType)
        {  
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiresAt = tokenType == TokenType.Access ? DateTime.UtcNow.AddMinutes(15) : DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: expiresAt,
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResult<string>> ValidateTokenAsync(string token, TokenType tokenType)
        {
            string typeOfToken = tokenType.ToString().ToLower();

            if (!_jwtHandler.CanReadToken(token))
            {
                return ServiceResult<string>.Fail($"Unreadable {typeOfToken} token.", 400);
            }

            var jwt = _jwtHandler.ReadJwtToken(token);

            if (DateTime.UtcNow > jwt.ValidTo)
            {
                return ServiceResult<string>.Fail($"Expired {typeOfToken} token.", 401);
            }

            if (tokenType == TokenType.Refresh)
            {
                var matchedToken = await _userRepo.GetRefreshTokenAsync(token);
                if (matchedToken == null)
                {
                    return ServiceResult<string>.Fail("Invalid refresh token.", 401);
                }
            }

            return ServiceResult<string>.Success(token, 200);
        }

        public async Task<ServiceResult<bool>> IsSuperAdmin(string token)
        {
            var jwt = _jwtHandler.ReadJwtToken(token);
            var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role != UserRole.Admin.ToString()) return ServiceResult<bool>.Fail("User not an admin.", 401);

            var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null) return ServiceResult<bool>.Fail("No email in the token.", 400);
            var matchedUser = await _userRepo.GetUserByEmailAsync(email);
            if (matchedUser == null) return ServiceResult<bool>.Fail("No user found.", 404);
            var matchedAdmin = await _adminRepo.GetAdminByUserIdAsync(matchedUser.Id);
            if (matchedAdmin == null) return ServiceResult<bool>.Fail("No admin record found.", 404);

            return ServiceResult<bool>.Success(matchedAdmin.IsSuperAdmin, 200);
        }
    }
}
