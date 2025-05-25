using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;

public class AuthServices : IAuthServices
{
    private readonly IAuthRepository _authRepo;
    private readonly IUserRepository _userRepo;
    private readonly IValidationServices _validationServices;
    private readonly ICustomerRepository _customerRepo;

    public AuthServices(IAuthRepository authRepo, IValidationServices validationServices, IUserRepository userRepo, ICustomerRepository customerRepo)
    {
        _userRepo = userRepo;
        _authRepo = authRepo;
        _validationServices = validationServices;
        _customerRepo = customerRepo;
    }

    public async Task<ServiceResult<AdminLoginResponse>?> AdminLoginAsync(LoginRequest req)
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

    public async Task<ServiceResult<CustomerLoginResponse>?> CustomerLoginAsync(LoginRequest req)
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

    public async Task<ServiceResult<CustomerRegisterResponse>?> CustomerRegisterAsync(CustomerRegisterRequest req)
    {
        var validationResult = _validationServices.ValidateCustomerRegistration(req);
        if (!validationResult.OK)
            return ServiceResult<CustomerRegisterResponse>.Fail(validationResult.ErrorMessage, validationResult.StatusCode);

        var mappedUser = CustomerMappers.CustomerRegisterToUserModel(req);
        await _userRepo.AddNewUserAsync(mappedUser);

        var mappedCustomer = CustomerMappers.CustomerRegisterToCustomerModel(req, mappedUser);
        await _customerRepo.AddNewCustomerAsync(mappedCustomer);

        var addressResult = await GetOrCreateCustomerAddressAsync(req.CustomerAddress, mappedCustomer);
        if (!addressResult.OK)
            return ServiceResult<CustomerRegisterResponse>.Fail(addressResult.ErrorMessage, addressResult.StatusCode);

        var response = CustomerMappers.CustomerRegisterModelsToResponse(mappedUser, mappedCustomer, addressResult.Data);

        return ServiceResult<CustomerRegisterResponse>.Success(response, 200);
    }

    public bool VerifyPassword(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }

    public async Task<ServiceResult<CustomerAddress>> GetOrCreateCustomerAddressAsync(CustomerAddressCreateRequest reqAddress, Customer customer)
    {
        CustomerAddress address;
        CustomerAddressDetail detail;

        bool addressExists = _customerRepo.AddressExists(reqAddress);

        if (addressExists)
        {
            address = await _customerRepo.GetCustomerAddressByReqAsync(reqAddress);
        }
        else
        {
            address = await _customerRepo.AddNewCustomerAddressAsync(reqAddress);
        }

        if (address == null)
            return ServiceResult<CustomerAddress>.Fail("Failed to find or create address.", 500);

        detail = await _customerRepo.AddNewCustomerAddressDetailAsync(customer, address);

        if (detail == null)
            return ServiceResult<CustomerAddress>.Fail("Failed to add address detail.", 500);

        return ServiceResult<CustomerAddress>.Success(address, 200);
    }
}
