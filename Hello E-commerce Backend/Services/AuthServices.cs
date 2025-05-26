using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;
using System.Net;
using System.Threading.Tasks;

public class AuthServices : IAuthServices
{
    private readonly IAuthRepository _authRepo;
    private readonly IUserRepository _userRepo;
    private readonly IValidationServices _validationServices;
    private readonly ICustomerRepository _customerRepo;
    private readonly IJwtHelper _jwtHelper;

    public AuthServices(IJwtHelper jwtHelper, IAuthRepository authRepo, IValidationServices validationServices, IUserRepository userRepo, ICustomerRepository customerRepo)
    {
        _userRepo = userRepo;
        _authRepo = authRepo;
        _validationServices = validationServices;
        _customerRepo = customerRepo;
        _jwtHelper = jwtHelper;
    }

    public async Task<ServiceResult<AdminLoginResponse>> AdminLoginAsync(LoginRequest req)
    {
        // verify credentials
        var matchedUser = await _authRepo.GetUserByEmailAsync(req.Email);
        if (matchedUser == null) return ServiceResult<AdminLoginResponse>.Fail("The user with this email does not exists.", 404);

        var matchedAdmin = await _authRepo.GetAdminByUserIdAsync(matchedUser.Id);
        if (matchedAdmin == null) return ServiceResult<AdminLoginResponse>.Fail("This user is not an admin.", 404);

        string? hashed = matchedUser.Password;
        if (!VerifyPassword(req.Password, hashed))
            return ServiceResult<AdminLoginResponse>.Fail("The inputted password is wrong.", 401);

        // map the request to a login response and return
        AdminLoginResponse response = AdminMappers.ToAdminLoginResponse(matchedUser, matchedAdmin);

        // return success
        return ServiceResult<AdminLoginResponse>.Success(response, 200);
    }

    public async Task<ServiceResult<CustomerLoginResponse>> CustomerLoginAsync(LoginRequest req)
    {
        var matchedUser = await _authRepo.GetUserByEmailAsync(req.Email);
        if (matchedUser == null) return ServiceResult<CustomerLoginResponse>.Fail("The user with this email does not exists.", 404);

        var matchedCustomer = await _authRepo.GetCustomerByUserIdAsync(matchedUser.Id);
        if (matchedCustomer == null) return ServiceResult<CustomerLoginResponse>.Fail("This user is not likely to be an admin.", 404);

        string hashed = matchedUser.Password;
        if (!VerifyPassword(req.Password, hashed))
            return ServiceResult<CustomerLoginResponse>.Fail("The inputted password is wrong.", 401);


        // map and return success
        CustomerLoginResponse response = CustomerMappers.ToCustomerLoginResponse(matchedUser, matchedCustomer);
        return ServiceResult<CustomerLoginResponse>.Success(response, 200);
    }

    public async Task<ServiceResult<CustomerRegisterResponse>> CustomerRegisterAsync(CustomerRegisterRequest req)
    {
        var validationResult = _validationServices.ValidateCustomerRegistration(req);
        if (!validationResult.OK)
            return ServiceResult<CustomerRegisterResponse>.Fail(validationResult.ErrorMessage, validationResult.StatusCode);

        var mappedUser = CustomerMappers.CustomerRegisterToUserModel(req);
        var mappedCustomer = CustomerMappers.CustomerRegisterToCustomerModel(req, mappedUser);
        var mappedAddress = CustomerMappers.CustomerAddressRegisterToModel(req.CustomerAddress);
        var formattedAddress = _validationServices.ReformatAddress(mappedAddress);

        var isAddressExisting = _customerRepo.AddressExists(formattedAddress);
        var existingAddress = isAddressExisting
            ? await _customerRepo.GetCustomerAddressByAddressAsync(formattedAddress)
            : null;

        await _userRepo.AddNewUserAsync(mappedUser);
        await _customerRepo.AddNewCustomerAsync(mappedCustomer);

        if (!isAddressExisting)
        {
            await _customerRepo.AddNewCustomerAddressAsync(formattedAddress);
            await _customerRepo.AddNewCustomerAddressDetailAsync(mappedCustomer, formattedAddress);
        }
        else
        {
            await _customerRepo.AddNewCustomerAddressDetailAsync(mappedCustomer, existingAddress);
        }

        var response = CustomerMappers.CustomerRegisterModelsToResponse(mappedUser, mappedCustomer, formattedAddress);
        return ServiceResult<CustomerRegisterResponse>.Success(response, 200);
    }

    public async Task<ServiceResult<TokenPair>> GetTokens(string email)
    {
        var user = await _userRepo.GetUserByEmailAsync(email);
        if (user == null)
            return ServiceResult<TokenPair>.Fail("The user with this email does not exist.", 404);

        var refreshToken = _jwtHelper.GenerateToken(user, TokenType.Refresh);
        var accessToken = _jwtHelper.GenerateToken(user, TokenType.Access);

        var tokens = new TokenPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return ServiceResult<TokenPair>.Success(tokens, 200);
    }


    public bool VerifyPassword(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}
