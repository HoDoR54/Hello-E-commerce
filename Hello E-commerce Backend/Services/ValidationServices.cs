using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Services
{
    public class ValidationServices : IValidationServices
    {
        public ServiceResult<CustomerRegisterRequest> ValidateCustomerRegistration(CustomerRegisterRequest req)
        {
            var result = ValidateEmail(req.Email);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}");

            result = ValidatePassword(req.Password);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}");

            result = ValidatePhoneNumber(req.PhoneNumber);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}");

            var addressResult = ValidateAddress(req.CustomerAddress);
            if (!addressResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}");

            return ServiceResult<CustomerRegisterRequest>.Success(req);
        }

        public ServiceResult<CustomerAddressCreateRequest> ValidateAddress(CustomerAddressCreateRequest address)
        {
            if (address == null)
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Address is required.");

            if (string.IsNullOrWhiteSpace(address.Street))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Street is required.");

            if (string.IsNullOrWhiteSpace(address.City))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("City is required.");

            if (string.IsNullOrWhiteSpace(address.Country))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Country is required.");

            if (string.IsNullOrWhiteSpace(address.PostalCode))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Postal code is required.");

            return ServiceResult<CustomerAddressCreateRequest>.Success(address);
        }

        public ServiceResult<string> ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ServiceResult<string>.Fail("Email is required.");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return ServiceResult<string>.Fail("Invalid email format.");
            }
            catch
            {
                return ServiceResult<string>.Fail("Invalid email format.");
            }

            return ServiceResult<string>.Success(email);
        }

        public ServiceResult<string> ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return ServiceResult<string>.Fail("Password is required.");

            if (password.Length < 8)
                return ServiceResult<string>.Fail("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                return ServiceResult<string>.Fail("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                return ServiceResult<string>.Fail("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                return ServiceResult<string>.Fail("Password must contain at least one digit.");

            return ServiceResult<string>.Success(password);
        }


        public ServiceResult<string> ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ServiceResult<string>.Fail("Phone number is required.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\+?[0-9]{7,15}$"))
                return ServiceResult<string>.Fail("Invalid phone number format.");

            return ServiceResult<string>.Success(phoneNumber);
        }

    }
}
