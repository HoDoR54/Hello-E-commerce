using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;
using System.Globalization;
using System.IO;
using System.Security.Principal;

namespace Services
{
    public class ValidationServices : IValidationServices
    {
        public ServiceResult<CustomerRegisterRequest> ValidateCustomerRegistration(CustomerRegisterRequest req)
        {
            var result = ValidateEmail(req.Email);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}", result.StatusCode);

            result = ValidatePassword(req.Password);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}", result.StatusCode);

            result = ValidatePhoneNumber(req.PhoneNumber);
            if (!result.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{result.ErrorMessage}", result.StatusCode);

            var addressResult = ValidateAddress(req.CustomerAddress);
            if (!addressResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail($"{addressResult.ErrorMessage}", result.StatusCode);

            return ServiceResult<CustomerRegisterRequest>.Success(req, 200);
        }

        public ServiceResult<CustomerAddressCreateRequest> ValidateAddress(CustomerAddressCreateRequest address)
        {
            if (address == null)
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Address is required.", 400);

            if (string.IsNullOrWhiteSpace(address.Street))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Street is required.", 400);

            if (string.IsNullOrWhiteSpace(address.City))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("City is required.", 400);

            if (string.IsNullOrWhiteSpace(address.Country))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Country is required.", 400);

            if (string.IsNullOrWhiteSpace(address.PostalCode))
                return ServiceResult<CustomerAddressCreateRequest>.Fail("Postal code is required.", 400);

            return ServiceResult<CustomerAddressCreateRequest>.Success(address, 200);
        }

        public ServiceResult<string> ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ServiceResult<string>.Fail("Email is required.", 400);

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return ServiceResult<string>.Fail("Invalid email format.", 400);
            }
            catch
            {
                return ServiceResult<string>.Fail("Invalid email format.", 400);
            }

            return ServiceResult<string>.Success(email, 200);
        }

        public ServiceResult<string> ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return ServiceResult<string>.Fail("Password is required.", 400);

            if (password.Length < 8)
                return ServiceResult<string>.Fail("Password must be at least 8 characters long.", 400);

            if (!password.Any(char.IsUpper))
                return ServiceResult<string>.Fail("Password must contain at least one uppercase letter.", 400);

            if (!password.Any(char.IsLower))
                return ServiceResult<string>.Fail("Password must contain at least one lowercase letter.", 400);

            if (!password.Any(char.IsDigit))
                return ServiceResult<string>.Fail("Password must contain at least one digit.", 400);

            return ServiceResult<string>.Success(password, 200);
        }

        public ServiceResult<string> ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ServiceResult<string>.Fail("Phone number is required.", 400);

            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\+?[0-9]{7,15}$"))
                return ServiceResult<string>.Fail("Invalid phone number format.", 400);

            return ServiceResult<string>.Success(phoneNumber, 200);
        }

        public CustomerAddress ReformatAddress(CustomerAddress address)
        {
            string streetLowered = address.Street.ToLower().Trim();
            string streetFormatted = streetLowered;

            if (streetLowered.EndsWith(".st"))
            {
                streetFormatted = streetLowered.Substring(0, streetLowered.Length - 3).Trim() + " Street";
            }
            else if (streetLowered.EndsWith(" st"))
            {
                streetFormatted = streetLowered.Substring(0, streetLowered.Length - 3).Trim() + " Street";
            }
            else if (streetLowered.EndsWith("street"))
            {
                streetFormatted = streetLowered.Substring(0, streetLowered.Length - 6).Trim() + " Street";
            }
            else if (streetLowered.EndsWith(" road"))
            {
                streetFormatted = streetLowered.Substring(0, streetLowered.Length - 5).Trim() + " Road";
            }
            else if (streetLowered.EndsWith(" rd."))
            {
                streetFormatted = streetLowered.Substring(0, streetLowered.Length - 4).Trim() + " Road";
            }

            streetFormatted = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(streetFormatted);

            string cityLowered = address.City.ToLower().Trim();
            string cityFormatted = cityLowered;

            if (cityLowered.EndsWith(" city"))
            {
                cityFormatted = cityLowered.Substring(0, cityLowered.Length - 5).Trim();
            }

            cityFormatted = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cityFormatted);
            string countryFormatted = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.Country);

            return new CustomerAddress
            {
                Street = streetFormatted,
                City = cityFormatted,
                Country = countryFormatted,
                PostalCode = address.PostalCode,
            };
        }
    }

}
