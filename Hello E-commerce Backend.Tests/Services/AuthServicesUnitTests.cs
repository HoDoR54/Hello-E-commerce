using Xunit;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Helpers;
using Services;
using Azure;

namespace Hello_E_commerce_Backend.Tests.Services
{
    public class AuthServicesUnitTests
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IAuthRepository _authRepo;
        private readonly IValidationServices _validator;
        private readonly IUserRepository _userRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICustomerMapper _customerMapper;
        private readonly IAdminMapper _adminMapper;
        private readonly AuthServices _service;

        public AuthServicesUnitTests()
        {
            _jwtHelper = A.Fake<IJwtHelper>();
            _authRepo = A.Fake<IAuthRepository>();
            _validator = A.Fake<IValidationServices>();
            _userRepo = A.Fake<IUserRepository>();
            _customerRepo = A.Fake<ICustomerRepository>();
            _passwordHasher = A.Fake<IPasswordHasher>();
            _customerMapper = A.Fake<ICustomerMapper>();
            _adminMapper = A.Fake<IAdminMapper>();

            _service = new AuthServices(
                _jwtHelper, _authRepo, _validator,
                _userRepo, _customerRepo, _passwordHasher,
                _customerMapper, _adminMapper
            );
        }

        // 1.1: Happy Path
        [Fact]
        public async Task LoginAsAdminAsync_WithValidCredentials_ShouldReturnSuccess ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingAdmin@gamil.com",
                Password = "testingAdmin123!@#"
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
                Name = "Mr. Testing Admin Hello",
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

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetAdminByUserIdAsync(userId)).Returns(admin);
            A.CallTo(() => _passwordHasher.Verify(request.Password, user.Password)).Returns(true);
            A.CallTo(() => _adminMapper.ToAdminLoginResponse(user, admin)).Returns(expectedResponse);

            // Act
            var result = await _service.LoginAsAdminAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.OK.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(expectedResponse);
        }

        // 1.2: Email not found
        [Fact]
        public async Task LoginAsAdminAsync_WithNonExistentEmail_ShouldReturnNotFound ()
        {
            var request = new LoginRequest
            {
                Email = "testingAdmin@gamil.com",
                Password = "testingAdmin123!@#"
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns((User?)null);

            // Act
            var result = await _service.LoginAsAdminAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.OK.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.ErrorMessage.Should().Be("User not found.");
            result.Data.Should().BeNull();
        }

        // 1.3: User not admin
        [Fact]
        public async Task LoginAsAdminAsync_UserNotAdmin_ShouldReturnNotFound ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingAdmin@gamil.com",
                Password = "testingAdmin123!@#"
            };

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Email = request.Email,
                Password = "hashedPassword",
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Customer,
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetAdminByUserIdAsync(user.Id)).Returns((Admin?)null);

            // Act
            var result = await _service.LoginAsAdminAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.OK.Should().BeFalse();
            result.ErrorMessage.Should().Be("User is not an admin.");
            result.StatusCode.Should().Be(404);
            result.Data.Should().BeNull();
        }

        // 1.4: Wrong password
        [Fact]
        public async Task LoginAsAdminAsync_WrongPassword_ShouldReturnUnauthorized ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingAdmin@gamil.com",
                Password = "testingAdmin123!@#"
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
                Name = "Mr. Testing Admin Hello",
                PhoneNumber = "09987654321",
                IsSuperAdmin = true
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetAdminByUserIdAsync(userId)).Returns(admin);
            A.CallTo(() => _passwordHasher.Verify(request.Password, user.Password)).Returns(false);

            // Act 
            var result = await _service.LoginAsAdminAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.OK.Should().BeFalse();
            result.StatusCode.Should().Be(401);
            result.ErrorMessage.Should().Be("Incorrect password.");
        }

        // 2.1: Happy Path
        [Fact]
        public async Task LoginAsCustomerAsync_WithValidCredentials_ShouldReturnSuccess ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingCustomer@gmail.com",
                Password = "testingCustomer123!@#"
            };

            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Email = request.Email,
            };

            var customer = new Customer
            {
                Id = customerId,
                Name = "Mr. Customer Hehe",
                PhoneNumber = "09123456789",
                DateOfBirth = new DateTime(2007, 4, 5),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsWarned = false,
                IsBanned = false,
                LoyaltyPoints = 0,
            };

            var expectedResponse = new CustomerLoginResponse
            {
                UserId = userId,
                Email = request.Email,
                CustomerId = customerId,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth= customer.DateOfBirth,
                CreatedAt= customer.CreatedAt,
                UpdatedAt= customer.UpdatedAt,
                IsWarned = customer.IsWarned,
                IsBanned = customer.IsBanned,
                LoyaltyPoints = customer.LoyaltyPoints,
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetCustomerByUserIdAsync(userId)).Returns(customer);
            A.CallTo(() => _passwordHasher.Verify(request.Password, user.Password)).Returns(true);
            A.CallTo(() => _customerMapper.ToCustomerLoginResponse(user, customer)).Returns(expectedResponse);

            // Act
            var result = await _service.LoginAsCustomerAsync(request);

            // Assert
            result.OK.Should().BeTrue();
            result.StatusCode.Should().Be(200);
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expectedResponse);
        }

        // 2.2: User not found
        [Fact]
        public async Task LoginAsCustomerAsync_WithNonExistentEmail_ShouldReturnNotFound ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingCustomer@gmail.com",
                Password = "testingCustomer123!@#"
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns((User?)null);

            // Act
            var result = await _service.LoginAsCustomerAsync(request);

            // Assert
            result.OK.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.ErrorMessage.Should().Be("User not found.");
            result.Data.Should().BeNull();
        }

        // 2.3: User not a customer
        [Fact]
        public async Task LoginAsCustomerAsync_UserNotCustomer_SholudReturnNotFound ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingCustomer@gmail.com",
                Password = "testingCustomer123!@#"
            };

            var userId = Guid.NewGuid();
            var user = new User
            {
                Email = request.Email,
                Role = UserRole.Admin
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetCustomerByUserIdAsync(user.Id)).Returns((Customer?)null);

            // Act
            var result = await _service.LoginAsCustomerAsync(request);

            // Assert
            result.OK.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.ErrorMessage.Should().Be("User is not a customer.");
            result.Data.Should().BeNull();
        }

        // 2.4: Wrong password
        [Fact]
        public async Task LoginAsCustomerAsync_WrongPassword_ShouldReturnUnauthorized ()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "testingCustomer@gmail.com",
                Password = "testingCustomer123!@#",
            };

            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var user = new User
            {
                Email = request.Email,
                Role = UserRole.Customer,
                Password = "hashedPassword",
            };

            var customer = new Customer
            {
                Id = customerId,
                User = user,
                UserId = userId,
            };

            A.CallTo(() => _authRepo.GetUserByEmailAsync(request.Email)).Returns(user);
            A.CallTo(() => _authRepo.GetCustomerByUserIdAsync(userId)).Returns(customer);
            A.CallTo(() => _passwordHasher.Verify(request.Password, user.Password)).Returns(false);

            // Act
            var result = await _service.LoginAsCustomerAsync(request);

            // Assert
            result.OK.Should().BeFalse();
            result.StatusCode.Should().Be(401);
            result.ErrorMessage.Should().Be("Incorrect password.");
            result.Data.Should().BeNull();
        }

        // 3.1: Happy Path
        [Fact]       
        public async Task RegisterCustomerAsync_WithValidCredentials_ShouldReturnSuccess()
        {
            // Arrange
            var now = DateTime.UtcNow;

            var request = new CustomerRegisterRequest
            {
                Email = "testingCustomer@gmail.com",
                Password = "testingCustomer123!@#",
                Name = "Mr. Testing Hehe",
                PhoneNumber = "09123456789",
                DateOfBirth = now,
                CustomerAddress = new CustomerAddressCreateRequest
                {
                    Street = "123 Street",
                    City = "City",
                    Country = "Country",
                    PostalCode = "12345"
                }
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = "hashedPassword",
            };

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
            };

            var address = new CustomerAddress
            {
                Id = Guid.NewGuid(),
                Street = "123 Street",
                City = "City",
                Country = "Country",
                PostalCode = "12345"
            };

            var addressResponse = new CustomerAddressResponse
            {
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode
            };

            var expectedResponse = new CustomerRegisterResponse
            {
                UserId = user.Id,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = now,
                CustomerAddress = addressResponse
            };

            // Set up mocks
            A.CallTo(() => _validator.ValidateCustomerRegistration(request))
                .Returns(ServiceResult<CustomerRegisterRequest>.Success(request, 200));

            A.CallTo(() => _customerMapper.CustomerRegisterToUserModel(request))
                .Returns(user);

            A.CallTo(() => _customerMapper.CustomerRegisterToCustomerModel(request, user))
                .Returns(customer);

            A.CallTo(() => _customerMapper.CustomerAddressRegisterToModel(request.CustomerAddress))
                .Returns(address);

            A.CallTo(() => _validator.ReformatAddress(address))
                .Returns(address);

            A.CallTo(() => _customerRepo.AddressExistsAsync(address))
                .Returns(false);

            A.CallTo(() => _customerMapper.CustomerRegisterModelsToResponse(user, customer, address))
                .Returns(expectedResponse);

            // Act
            var result = await _service.RegisterCustomerAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.OK.Should().BeTrue();
            result.StatusCode.Should().Be(200);
            result.Data.Should().BeEquivalentTo(expectedResponse);

            A.CallTo(() => _userRepo.AddNewUserAsync(user)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _customerRepo.AddNewCustomerAsync(customer)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _customerRepo.AddNewCustomerAddressAsync(address)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _customerRepo.AddNewCustomerAddressDetailAsync(customer, address)).MustHaveHappenedOnceExactly();
        }
    }
}
