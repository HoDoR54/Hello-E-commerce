using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;
using System.Globalization;
using System.IO;
using System.Security.Principal;

namespace Services
{
    public class ValidationService : IValidationService
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IAdminRepository _adminRepo;
        private readonly IUserRepository _userRepo;
        public ValidationService(IJwtHelper jwtHelper, IAdminRepository adminRepo, IUserRepository userRepo)
        {
            _adminRepo = adminRepo;
            _jwtHelper = jwtHelper;
            _userRepo = userRepo;
        }
        public ServiceResult<CustomerRegisterRequest> ValidateCustomerRegistration(CustomerRegisterRequest req)
        {
            if (req == null)
                return ServiceResult<CustomerRegisterRequest>.Fail("Request cannot be null.", 400);

            var emailResult = ValidateEmail(req.Email);
            if (!emailResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail(emailResult.ErrorMessage, emailResult.StatusCode);

            var passwordResult = ValidatePassword(req.Password);
            if (!passwordResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail(passwordResult.ErrorMessage, passwordResult.StatusCode);

            var phoneResult = ValidatePhoneNumber(req.PhoneNumber);
            if (!phoneResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail(phoneResult.ErrorMessage, phoneResult.StatusCode);

            var addressResult = ValidateAddress(req.CustomerAddress);
            if (!addressResult.OK)
                return ServiceResult<CustomerRegisterRequest>.Fail(addressResult.ErrorMessage, addressResult.StatusCode);

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

        public ServiceResult<CreateAdminRequest> ValidateCreateAdminRequest(CreateAdminRequest req)
        {
            if (req == null)
                return ServiceResult<CreateAdminRequest>.Fail("Request cannot be null.", 400);

            var emailResult = ValidateEmail(req.Email);
            if (!emailResult.OK)
                return ServiceResult<CreateAdminRequest>.Fail(emailResult.ErrorMessage, emailResult.StatusCode);

            var phoneResult = ValidatePhoneNumber(req.PhoneNumber);
            if (!phoneResult.OK)
                return ServiceResult<CreateAdminRequest>.Fail(phoneResult.ErrorMessage, phoneResult.StatusCode);

            return ServiceResult<CreateAdminRequest>.Success(req, 200);
        }

        public async Task<ServiceResult<Admin>> ValidateAndReturnSuperAdminAsync(Guid UserId)
        {
            var matchedUser = await _userRepo.GetUserByIdAsync(UserId);
            if (matchedUser == null) return ServiceResult<Admin>.Fail("No user found", 404);

            var matchedAdmin = await _adminRepo.GetAdminByUserIdAsync(matchedUser.Id);
            if (matchedAdmin == null) return ServiceResult<Admin>.Fail("No admin found.", 404);

            if (!matchedAdmin.IsSuperAdmin) return ServiceResult<Admin>.Fail("User is not a super admin.", 401);

            return ServiceResult<Admin>.Success(matchedAdmin, 200);
        }
    }

}
