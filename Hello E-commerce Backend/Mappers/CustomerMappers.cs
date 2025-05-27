using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public static class CustomerMappers
    {
        public static CustomerLoginResponse ToCustomerLoginResponse(User user, Customer cus)
        {
            return new CustomerLoginResponse
            {
                UserId = user.Id,
                Email = user.Email,

                CustomerId = cus.Id,
                Name = cus.Name,
                PhoneNumber = cus.PhoneNumber,
                DateOfBirth = cus.DateOfBirth,
                CreatedAt = cus.CreatedAt,
                UpdatedAt = cus.UpdatedAt,
                IsWarned = cus.IsWarned,
                WarningLevel = cus.WarningLevel,
                IsBanned = cus.IsBanned,
                BannedDays = cus.BannedDays,
                LoyaltyPoints = cus.LoyaltyPoints
            };
        }

        public static User CustomerRegisterToUserModel (CustomerRegisterRequest req)
        {
            return new User
            {
                Id = new Guid(),
                Email = req.Email,
                // hash the password
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = UserRole.Customer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        public static Customer CustomerRegisterToCustomerModel (CustomerRegisterRequest req, User user)
        {
            return new Customer
            {
                Id = new Guid(),
                UserId = user.Id,
                User = user,
                Name = req.Name,
                PhoneNumber = req.PhoneNumber,
                DateOfBirth = req.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsWarned = false,
                WarningLevel = 0,
                IsBanned = false,
                BannedDays = 0,
                LoyaltyPoints = 0,
            };
        }

        public static CustomerAddress CustomerAddressRegisterToModel (CustomerAddressCreateRequest address)
        {
            return new CustomerAddress
            {
                Id = new Guid(),
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
            };
        }

        public static CustomerAddressResponse CustomerAddressModelToResponse (CustomerAddress address)
        {
            return new CustomerAddressResponse
            {
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
            };
        }

        public static CustomerRegisterResponse CustomerRegisterModelsToResponse (User user,  Customer customer, CustomerAddress address)
        {
            return new CustomerRegisterResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth= customer.DateOfBirth,
                CustomerAddress = CustomerAddressModelToResponse(address),
            };
        }
    }
}
