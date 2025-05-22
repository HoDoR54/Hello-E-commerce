using E_commerce_Admin_Dashboard.DTO.Responses;
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

    }
}
