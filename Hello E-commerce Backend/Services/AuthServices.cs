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
        if (matchedUser == null) return ServiceResult<AdminLoginResponse>.Fail("The user with this email does not exists.");

        var matchedAdmin = await _authRepo.GetAdminByUserIdAsync(matchedUser.Id);
        if (matchedAdmin == null) return ServiceResult<AdminLoginResponse>.Fail("This user is not an admin.");

        string? hashed = matchedUser.Password;
        if (!VerifyPassword(req.Password, hashed))
            return ServiceResult<AdminLoginResponse>.Fail("The inputted password is wrong.");

        // map the request to a login response and return
        AdminLoginResponse response = AdminMappers.ToAdminLoginResponse(matchedUser, matchedAdmin);
        // return success
        return ServiceResult<AdminLoginResponse>.Success(response);
    }

    public async Task<ServiceResult<CustomerLoginResponse>?> CustomerLoginAsync(LoginRequest req)
    {
        var matchedUser = await _authRepo.GetUserByEmailAsync(req.Email);
        if (matchedUser == null) return ServiceResult<CustomerLoginResponse>.Fail("The user with this email does not exists.");

        var matchedCustomer = await _authRepo.GetCustomerByUserIdAsync(matchedUser.Id);
        if (matchedCustomer == null) return ServiceResult<CustomerLoginResponse>.Fail("This user is not an admin.");

        string hashed = matchedUser.Password;
        if (!VerifyPassword(req.Password, hashed))
            return ServiceResult<CustomerLoginResponse>.Fail("The inputted password is wrong.");


        // map and return success
        CustomerLoginResponse response = CustomerMappers.ToCustomerLoginResponse(matchedUser, matchedCustomer);
        return ServiceResult<CustomerLoginResponse>.Success(response);
    }

    public async Task<ServiceResult<CustomerRegisterResponse>?> CustomerRegisterAsync(CustomerRegisterRequest req)
    {
        var result = _validationServices.ValidateCustomerRegistration(req);
        if (!result.OK)
            return ServiceResult<CustomerRegisterResponse>.Fail($"{result.ErrorMessage}");

        User mappedUser = CustomerMappers.CustomerRegisterToUserModel(req);
        await _userRepo.AddNewUserAsync(mappedUser);

        Customer mappedCustomer = CustomerMappers.CustomerRegisterToCustomerModel(req, mappedUser);
        await _customerRepo.AddNewCustomerAsync(mappedCustomer);

        CustomerAddress address;
        CustomerAddressDetail addressDetail;
        bool addressExists = _customerRepo.AddressExists(req.CustomerAddress); 
        if (addressExists)
        {
            address = await _customerRepo.GetCustomerAddressByReqAsync(req.CustomerAddress);
            addressDetail = await _customerRepo.AddNewCustomerAddressDetailAsync(mappedCustomer, address);
        }
        else
        {
            address = await _customerRepo.AddNewCustomerAddressAsync(req.CustomerAddress);
            addressDetail = await _customerRepo.AddNewCustomerAddressDetailAsync(mappedCustomer, address);
        }

        if (addressDetail == null || address == null)
            return ServiceResult<CustomerRegisterResponse>.Fail("Failed to add the address.");

        CustomerRegisterResponse response = CustomerMappers.CustomerRegisterModelsToResponse(mappedUser, mappedCustomer, address);

        return ServiceResult<CustomerRegisterResponse>.Success(response);
    }

    public bool VerifyPassword(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}
