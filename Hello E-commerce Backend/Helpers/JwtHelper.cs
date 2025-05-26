using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

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
        public JwtHelper (IConfiguration configuration)
        {
            _configuration = configuration;
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
    }
}
