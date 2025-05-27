using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Admin_Dashboard.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerAddressDetail> AddNewCustomerAddressDetailAsync(Customer customer, CustomerAddress address)
        {
            CustomerAddressDetail customerAddressDetail = new CustomerAddressDetail
            {
                Id = new Guid(),
                CustomerId = customer.Id,
                AddressId = address.Id,
                Customer = customer,
                CustomerAddress = address,
            };

            await _context.CustomerAddressDetails.AddAsync(customerAddressDetail);
            await _context.SaveChangesAsync();
            return customerAddressDetail;
        }

        public async Task<Customer> AddNewCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public bool AddressExists(CustomerAddress address)
        {
            return _context.CustomerAddresses.Any(a =>
                a.Street == address.Street &&
                a.City == address.City &&
                a.Country == address.Country &&
                a.PostalCode == address.PostalCode);
        }

        public async Task<CustomerAddress?> GetCustomerAddressByAddressAsync(CustomerAddress address)
        {
            return await _context.CustomerAddresses.FirstOrDefaultAsync(a =>
                a.Street == address.Street &&
                a.City == address.City &&
                a.Country == address.Country &&
                a.PostalCode == address.PostalCode);
        }

        public async Task<CustomerAddress> AddNewCustomerAddressAsync(CustomerAddress address)
        {
            await _context.CustomerAddresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
