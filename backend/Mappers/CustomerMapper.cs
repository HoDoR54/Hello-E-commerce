using E_commerce_Admin_Dashboard.DTO.Requests.Customers;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Customers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;

public class CustomerMapper : ICustomerMapper
{
    private readonly IPasswordHasher _passwordHasher;

    public CustomerMapper(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public CustomerResponse ToCustomerResponse(User user, Customer cus)
    {
        UserResponse userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsWarned = user.IsWarned,
            WarningLevel = user.WarningLevel,
            IsBanned = user.IsBanned,
            BannedDays = user.BannedDays,
        };
        return new CustomerResponse
        {
            User = userResponse,
            CustomerId = cus.Id,
            Name = cus.Name,
            DateOfBirth = cus.DateOfBirth,
            LoyaltyPoints = cus.LoyaltyPoints
        };
    }

    public User CustomerRegisterToUserModel(CustomerRegisterRequest req)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email,
            PhoneNumber = req.PhoneNumber,
            Password = _passwordHasher.Hash(req.Password),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
            IsWarned = false,
            IsBanned = false,
        };
    }

    public Customer CustomerRegisterToCustomerModel(CustomerRegisterRequest req, User user)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            Name = req.Name,
            DateOfBirth = req.DateOfBirth,
            IsDeleted = false,
            LoyaltyPoints = 0,
        };
    }

    public CustomerAddress CustomerAddressRegisterToModel(CustomerAddressCreateRequest address)
    {
        return new CustomerAddress
        {
            Id = Guid.NewGuid(),
            Street = address.Street,
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
        };
    }

    public CustomerAddressResponse CustomerAddressModelToResponse(CustomerAddress address)
    {
        return new CustomerAddressResponse
        {
            Street = address.Street,
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
        };
    }

    public  CustomerResponse ToCustomerResponse(User user, Customer customer, CustomerAddress address)
    {
        UserResponse userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsWarned = user.IsWarned,
            WarningLevel = user.WarningLevel,
            IsBanned = user.IsBanned,
            BannedDays = user.BannedDays,
        };
        return new CustomerResponse
        {
            User = userResponse,
            CustomerId = customer.Id,
            PhoneNumber = user.PhoneNumber,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Name = customer.Name,
            DateOfBirth = customer.DateOfBirth,
            LoyaltyPoints= customer.LoyaltyPoints,
        };
    }

    public CustomerAddressDetail AddressAndCustomerToCustomerAddressDetail(CustomerAddress address, Customer customer)
    {
        return new CustomerAddressDetail
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            AddressId = address.Id,
            Customer = customer,
            CustomerAddress = address,
        };
    }
}
