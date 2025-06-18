﻿using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
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

    public CustomerResponse ToCustomerLoginResponse(User user, Customer cus)
    {
        return new CustomerResponse
        {
            UserId = user.Id,
            Email = user.Email,
            CustomerId = cus.Id,
            Name = cus.Name,
            PhoneNumber = cus.PhoneNumber,
            DateOfBirth = cus.DateOfBirth,
            CreatedAt = cus.CreatedAt,
            UpdatedAt = cus.UpdatedAt,
            LoyaltyPoints = cus.LoyaltyPoints
        };
    }

    public User CustomerRegisterToUserModel(CustomerRegisterRequest req)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email,
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
            PhoneNumber = req.PhoneNumber,
            DateOfBirth = req.DateOfBirth,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
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

    public CustomerRessponse CustomerRegisterModelsToResponse(User user, Customer customer, CustomerAddress address)
    {
        return new CustomerRessponse
        {
            UserId = user.Id,
            Email = user.Email,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            CustomerAddress = CustomerAddressModelToResponse(address),
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
