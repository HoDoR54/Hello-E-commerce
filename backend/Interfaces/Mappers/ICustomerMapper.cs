using E_commerce_Admin_Dashboard.DTO.Requests.Customers;
using E_commerce_Admin_Dashboard.DTO.Responses.Customers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces.Mappers
{
    public interface ICustomerMapper
    {
        CustomerResponse ToCustomerResponse(User user, Customer customer);
        User CustomerRegisterToUserModel(CustomerRegisterRequest req);
        Customer CustomerRegisterToCustomerModel(CustomerRegisterRequest req, User user);
        CustomerAddress CustomerAddressRegisterToModel(CustomerAddressCreateRequest address);
        CustomerAddressResponse CustomerAddressModelToResponse(CustomerAddress address);

        CustomerAddressDetail AddressAndCustomerToCustomerAddressDetail (CustomerAddress address,  Customer customer);
        CustomerResponse ToCustomerResponse(User user, Customer customer, CustomerAddress address);
    }
}
