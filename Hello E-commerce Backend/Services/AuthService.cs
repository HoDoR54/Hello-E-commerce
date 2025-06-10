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
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;
        private readonly IValidationService _validator;
        private readonly ICustomerRepository _customerRepo;
        private readonly IJwtHelper _jwtHelper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAdminMapper _adminMapper;
        private readonly ICustomerMapper _customerMapper;
        private readonly IGeneralMapper _generalMapper;

        public AuthService(
        IJwtHelper jwtHelper,
        IAuthRepository authRepository,
        IValidationService validator,
        IUserRepository userRepository,
        ICustomerRepository customerRepo,
        IPasswordHasher passwordHasher,
        ICustomerMapper customerMapper,
        IAdminMapper adminMapper,
        IGeneralMapper generalMapper)
        {
            _userRepo = userRepository;
            _authRepo = authRepository;
            _validator = validator;
            _customerRepo = customerRepo;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
            _customerMapper = customerMapper;
            _adminMapper = adminMapper;
            _generalMapper = generalMapper;
        }

        public async Task<ServiceResult<AdminResponse>> LoginAsAdminAsync(LoginRequest request)
        {
            var user = await _authRepo.GetUserByEmailAsync(request.Email);
            if (user == null) return ServiceResult<AdminResponse>.Fail("User not found.", 404);

            var admin = await _authRepo.GetAdminByUserIdAsync(user.Id);
            if (admin == null) return ServiceResult<AdminResponse>.Fail("User is not an admin.", 404);

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return ServiceResult<AdminResponse>.Fail("Incorrect password.", 401);

            var response = _adminMapper.ToAdminLoginResponse(user, admin);
            return ServiceResult<AdminResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<CustomerLoginResponse>> LoginAsCustomerAsync(LoginRequest request)
        {
            var user = await _authRepo.GetUserByEmailAsync(request.Email);
            if (user == null) return ServiceResult<CustomerLoginResponse>.Fail("User not found.", 404);

            var customer = await _authRepo.GetCustomerByUserIdAsync(user.Id);
            if (customer == null) return ServiceResult<CustomerLoginResponse>.Fail("User is not a customer.", 404);

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return ServiceResult<CustomerLoginResponse>.Fail("Incorrect password.", 401);

            var response = _customerMapper.ToCustomerLoginResponse(user, customer);
            return ServiceResult<CustomerLoginResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<CustomerRegisterResponse>> RegisterCustomerAsync(CustomerRegisterRequest request)
        {
            var credentialsValidation = _validator.ValidateCustomerRegistration(request);
            if (!credentialsValidation.OK)
                return ServiceResult<CustomerRegisterResponse>.Fail(credentialsValidation.ErrorMessage, credentialsValidation.StatusCode);

            var newUser = _customerMapper.CustomerRegisterToUserModel(request);
            var newCustomer = _customerMapper.CustomerRegisterToCustomerModel(request, newUser);
            var newAddress = _customerMapper.CustomerAddressRegisterToModel(request.CustomerAddress);
            var formattedAddress = _validator.ReformatAddress(newAddress);

            var addressExists = await _customerRepo.AddressExistsAsync(formattedAddress);
            var existingAddress = addressExists
                ? await _customerRepo.GetCustomerAddressByAddressAsync(formattedAddress)
                : null;

            await _userRepo.AddNewUserAsync(newUser);
            await _customerRepo.AddNewCustomerAsync(newCustomer);

            if (existingAddress == null)
            {
                await _customerRepo.AddNewCustomerAddressAsync(formattedAddress);
                var customerAddressDetail = _customerMapper.AddressAndCustomerToCustomerAddressDetail(formattedAddress, newCustomer);
                await _customerRepo.AddNewCustomerAddressDetailAsync(customerAddressDetail);
            }
            else
            {
                var customerAddressDetail = _customerMapper.AddressAndCustomerToCustomerAddressDetail(existingAddress, newCustomer);
                await _customerRepo.AddNewCustomerAddressDetailAsync(customerAddressDetail);
            }

            var response = _customerMapper.CustomerRegisterModelsToResponse(newUser, newCustomer, formattedAddress);
            return ServiceResult<CustomerRegisterResponse>.Success(response, 201);
        }

        public async Task<ServiceResult<string>> GenerateTokenAsync(string? email, TokenType type)
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

            RefreshToken? matchedToken = await _userRepo.GetRefreshTokenAsync(refreshToken);
            if (matchedToken == null) return ServiceResult<string>.Fail("Inavlid refresh token.", 400);
            var matchedUser = await _userRepo.GetUserByIdAsync(matchedToken.UserId);
            if (matchedUser == null) return ServiceResult<string>.Fail("No user found.", 404);
            var email = matchedUser?.Email;
            var newAccessToken = await GenerateTokenAsync(email, TokenType.Access);
            if (!newAccessToken.OK)
                return ServiceResult<string>.Fail(newAccessToken.ErrorMessage, newAccessToken.StatusCode);

            return ServiceResult<string>.Success(newAccessToken.Data, 200);
        }

        public async Task<ServiceResult<TokenPair>> GenerateTokenPairAsync(string email)
        {
            var refreshResult = await GenerateTokenAsync(email, TokenType.Refresh);
            if (!refreshResult.OK) return ServiceResult<TokenPair>.Fail(refreshResult.ErrorMessage, refreshResult.StatusCode);
            await AddNewRefreshTokenAsync(refreshResult.Data);

            var accessResult = await GenerateTokenAsync(email, TokenType.Access);
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
            var user = await _userRepo.GetUserByEmailAsync(email);
            return user == null
                ? ServiceResult<User>.Fail("User not found.", 404)
                : ServiceResult<User>.Success(user, 200);
        }

        public async Task<ServiceResult<RefreshToken>> AddNewRefreshTokenAsync(string refreshToken)
        {
            var userId = _jwtHelper.GetUserIdByToken(refreshToken);
            var tokenModel = _generalMapper.RefreshTokenStringToModel(refreshToken, userId);
            var repoResult = await _authRepo.AddNewRefreshToken(tokenModel);
            if (repoResult == null) return ServiceResult<RefreshToken>.Fail("Adding the token failed.", 500);

            return ServiceResult<RefreshToken>.Success(repoResult, 201);
        }
    }

}
