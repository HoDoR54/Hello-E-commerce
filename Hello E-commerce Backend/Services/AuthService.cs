using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAdminMapper _adminMapper;
        private readonly ICustomerMapper _customerMapper;

        public AuthService(
        IJwtHelper jwtHelper,
        IAuthRepository authRepository,
        IValidationService validator,
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher,
        ICustomerMapper customerMapper,
        IAdminMapper adminMapper)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _validator = validator;
            _customerRepository = customerRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
            _customerMapper = customerMapper;
            _adminMapper = adminMapper;
        }

        public async Task<ServiceResult<AdminResponse>> LoginAsAdminAsync(LoginRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return ServiceResult<AdminResponse>.Fail("User not found.", 404);

            var admin = await _authRepository.GetAdminByUserIdAsync(user.Id);
            if (admin == null) return ServiceResult<AdminResponse>.Fail("User is not an admin.", 404);

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return ServiceResult<AdminResponse>.Fail("Incorrect password.", 401);

            var response = _adminMapper.ToAdminLoginResponse(user, admin);
            return ServiceResult<AdminResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<CustomerLoginResponse>> LoginAsCustomerAsync(LoginRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return ServiceResult<CustomerLoginResponse>.Fail("User not found.", 404);

            var customer = await _authRepository.GetCustomerByUserIdAsync(user.Id);
            if (customer == null) return ServiceResult<CustomerLoginResponse>.Fail("User is not a customer.", 404);

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return ServiceResult<CustomerLoginResponse>.Fail("Incorrect password.", 401);

            var response = _customerMapper.ToCustomerLoginResponse(user, customer);
            return ServiceResult<CustomerLoginResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<CustomerRegisterResponse>> RegisterCustomerAsync(CustomerRegisterRequest request)
        {
            var validation = _validator.ValidateCustomerRegistration(request);
            if (!validation.OK)
                return ServiceResult<CustomerRegisterResponse>.Fail(validation.ErrorMessage, validation.StatusCode);

            var newUser = _customerMapper.CustomerRegisterToUserModel(request);
            var newCustomer = _customerMapper.CustomerRegisterToCustomerModel(request, newUser);
            var newAddress = _customerMapper.CustomerAddressRegisterToModel(request.CustomerAddress);
            var formattedAddress = _validator.ReformatAddress(newAddress);

            var addressExists = await _customerRepository.AddressExistsAsync(formattedAddress);
            var existingAddress = addressExists
                ? await _customerRepository.GetCustomerAddressByAddressAsync(formattedAddress)
                : null;

            await _userRepository.AddNewUserAsync(newUser);
            await _customerRepository.AddNewCustomerAsync(newCustomer);

            if (!addressExists)
            {
                await _customerRepository.AddNewCustomerAddressAsync(formattedAddress);
                await _customerRepository.AddNewCustomerAddressDetailAsync(newCustomer, formattedAddress);
            }
            else
            {
                await _customerRepository.AddNewCustomerAddressDetailAsync(newCustomer, existingAddress);
            }

            var response = _customerMapper.CustomerRegisterModelsToResponse(newUser, newCustomer, formattedAddress);
            return ServiceResult<CustomerRegisterResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<string>> GenerateTokenAsync(string? email, TokenType type, string? refreshToken)
        {
            if (string.IsNullOrEmpty(email))
                return ServiceResult<string>.Fail("Email is required.", 400);

            var userResult = await GetUserByEmailAsync(email);
            if (!userResult.OK)
                return ServiceResult<string>.Fail(userResult.ErrorMessage, userResult.StatusCode);

            var token = _jwtHelper.GenerateToken(userResult.Data, type);
            return ServiceResult<string>.Success(token, 200);
        }

        public async Task<ServiceResult<string>> RefreshAccessToken(string refreshToken)
        {
            var refreshResult = await _jwtHelper.ValidateTokenAsync(refreshToken, TokenType.Refresh);
            if (!refreshResult.OK)
                return ServiceResult<string>.Fail(refreshResult.ErrorMessage, refreshResult.StatusCode);

            RefreshToken matchedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
            string email = matchedToken.User.Email;
            var newAccessToken = await GenerateTokenAsync(email, TokenType.Access, refreshToken);
            if (!newAccessToken.OK)
                return ServiceResult<string>.Fail(newAccessToken.ErrorMessage, newAccessToken.StatusCode);

            return ServiceResult<string>.Success(newAccessToken.Data, 200);
        }

        public async Task<ServiceResult<TokenPair>> GenerateTokenPairAsync(string email)
        {
            var refreshResult = await GenerateTokenAsync(email, TokenType.Refresh, null);
            if (!refreshResult.OK) return ServiceResult<TokenPair>.Fail(refreshResult.ErrorMessage, refreshResult.StatusCode);

            var accessResult = await GenerateTokenAsync(email, TokenType.Access, null);
            if (!accessResult.OK) return ServiceResult<TokenPair>.Fail(accessResult.ErrorMessage, accessResult.StatusCode);

            var tokenPair = new TokenPair
            {
                AccessToken = accessResult.Data,
                RefreshToken = refreshResult.Data
            };

            return ServiceResult<TokenPair>.Success(tokenPair, 200);
        }

        public async Task<ServiceResult<string>> ValidateTokenAsync(string token, TokenType type)
        {
            var result = await _jwtHelper.ValidateTokenAsync(token, type);
            return result.OK
                ? ServiceResult<string>.Success(token, 200)
                : ServiceResult<string>.Fail(result.ErrorMessage, result.StatusCode);
        }

        public async Task<ServiceResult<User>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null
                ? ServiceResult<User>.Fail("User not found.", 404)
                : ServiceResult<User>.Success(user, 200);
        }
    }

}
