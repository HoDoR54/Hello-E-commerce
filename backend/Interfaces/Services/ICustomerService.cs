using E_commerce_Admin_Dashboard.DTO.Responses.Customers;
using E_commerce_Admin_Dashboard.Helpers;

namespace E_commerce_Admin_Dashboard.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<ServiceResult<CustomerResponse>> GetAllCustomersAsync(int? page, int? limit, string? search, string? sort);
    }
}
