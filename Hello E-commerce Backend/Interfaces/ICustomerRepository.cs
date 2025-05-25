using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddNewCustomerAsync(Customer customer);

        Task<CustomerAddressDetail> AddNewCustomerAddressDetailAsync(Customer customer, CustomerAddress address);

        bool AddressExists(CustomerAddress address);

        Task<CustomerAddress> GetCustomerAddressByAddressAsync(CustomerAddress address);

        Task<CustomerAddress> AddNewCustomerAddressAsync(CustomerAddress address);
    }
}
