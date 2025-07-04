using E_commerce_Admin_Dashboard.DTO.Responses.Customers;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Services;

namespace E_commerce_Admin_Dashboard.Services
{
    public class CustomerService : ICustomerService
    {
        public Task<ServiceResult<CustomerResponse>> GetAllCustomersAsync(int? page, int? limit, string? search, string? sort)
        {
            throw new NotImplementedException();
        }
    }
}
