using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Mappers
{
    public class AdminMapper : IAdminMapper
    {
        private readonly IPasswordHasher _passwordHasher;
        public AdminMapper (IPasswordHasher pwHashwer)
        {
            _passwordHasher = pwHashwer;
        }
        public Admin CreateAdminRequestToAdminModel(CreateAdminRequest request, User user, Admin superAdmin)
        {
            return new Admin
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
                Name = request.Name,
                IsDeleted = false,
                IsSuperAdmin = false,
                CreatedBy = superAdmin.Id,
                CreatedByAdmin = superAdmin
            };
        }

        public User CreateAdminRequestToUserModel(CreateAdminRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = _passwordHasher.Hash("temporaryPasswordForAdmins123!@#"),
                Role = UserRole.Admin,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsBanned = false,
                IsWarned = false,
            };
        }

        public AdminResponse ToAdminResponse(User user, Admin admin)
        {
            UserResponse userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber= user.PhoneNumber,
                CreatedAt   = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsWarned = user.IsWarned,
                WarningLevel = user.WarningLevel,
                IsBanned = user.IsBanned,
                BannedDays = user.BannedDays,
            };
            return new AdminResponse
            {
                User = userResponse,
                AdminId = admin.Id,
                Name = admin.Name,
                IsSuperAdmin = admin.IsSuperAdmin,
                CreatedBy  = admin.CreatedBy,
            };
        }
    }
}
