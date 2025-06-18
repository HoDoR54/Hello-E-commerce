using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Mappers
{
    public interface ICustomerMapper
    {
        CustomerResponse ToCustomerLoginResponse(User user, Customer customer);
        User CustomerRegisterToUserModel(CustomerRegisterRequest req);
        Customer CustomerRegisterToCustomerModel(CustomerRegisterRequest req, User user);
        CustomerAddress CustomerAddressRegisterToModel(CustomerAddressCreateRequest address);
        CustomerAddressResponse CustomerAddressModelToResponse(CustomerAddress address);

        CustomerAddressDetail AddressAndCustomerToCustomerAddressDetail (CustomerAddress address,  Customer customer);
        CustomerResponse CustomerRegisterModelsToResponse(User user, Customer customer, CustomerAddress address);
    }
}
