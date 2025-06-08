using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Services
{
    public interface IValidationService
    {
        ServiceResult<CustomerRegisterRequest> ValidateCustomerRegistration(CustomerRegisterRequest req);
        ServiceResult<string> ValidateEmail(string email);
        ServiceResult<string> ValidatePassword(string password);
        ServiceResult<string> ValidatePhoneNumber(string phoneNumber);
        ServiceResult<CustomerAddressCreateRequest> ValidateAddress(CustomerAddressCreateRequest address);
        CustomerAddress ReformatAddress(CustomerAddress address);

        Task<ServiceResult<bool>> ValidateSuperAdminRole(string token);
    }
}
