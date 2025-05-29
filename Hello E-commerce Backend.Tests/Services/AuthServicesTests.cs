using Xunit;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using E_commerce_Admin_Dashboard.Services;
using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;
using E_commerce_Admin_Dashboard.Interfaces.Services;

namespace Hello_E_commerce_Backend.Tests.Services
{
    public class AuthServicesTests
    {
        [Fact]
        public async Task LoginAsAdminAsync_WithValidCredentials_ShouldReturnSuccessResponse()
        {
            // Arrange
            var jwtHelper = A.Fake<IJwtHelper>();
            var authRepo = A.Fake<IAuthRepository>();
            var validator = A.Fake<IValidationServices>();
            var userRepo = A.Fake<IUserRepository>();
            var customerRepo = A.Fake<ICustomerRepository>();
            var passwordHasher = A.Fake<IPasswordHasher>();
            var customerMapper = A.Fake<ICustomerMapper>();
            var adminMapper = A.Fake<IAdminMapper>();

            var service = new AuthServices(
                jwtHelper, authRepo, validator,
                userRepo, customerRepo, passwordHasher,
                customerMapper, adminMapper
            );

            var request = new LoginRequest
            {
                Email = "admin@example.com",
                Password = "secure123"
            };

            var userId = Guid.NewGuid();
            var adminId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Email = request.Email,
                Password = "hashedPassword",
                CreatedAt = DateTime.UtcNow
            };

            var admin = new Admin
            {
                Id = adminId,
                UserId = userId,
                Name = "Admin Name",
                PhoneNumber = "09987654321",
                IsSuperAdmin = true
            };

            var expectedResponse = new AdminLoginResponse
            {
                UserId = userId,
                Email = user.Email,
                AdminId = adminId,
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                IsSuperAdmin = admin.IsSuperAdmin,
                CreatedAt = user.CreatedAt
            };

            A.CallTo(() => authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => authRepo.GetAdminByUserIdAsync(userId)).Returns(admin);
            A.CallTo(() => passwordHasher.Verify(request.Password, user.Password)).Returns(true);
            A.CallTo(() => adminMapper.ToAdminLoginResponse(user, admin)).Returns(expectedResponse);

            // Act
            var result = await service.LoginAsAdminAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.OK.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
