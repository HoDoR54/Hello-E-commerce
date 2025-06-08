using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IValidationService _validator;
        private readonly IAdminMapper _adminMapper;
        private readonly IUserRepository _userRepo;
        public AdminService(IAdminRepository adminRepo, IValidationService validationService, IAdminMapper adminMapper, IUserRepository userRepo)
        {
            _adminRepo = adminRepo;
            _validator = validationService;
            _adminMapper = adminMapper;
            _userRepo = userRepo;
        }

        public async Task<ServiceResult<List<AdminResponse>>> GetAllAdmins(string token ,string? search, int limit, int page, string? sort)
        {
            var validationResult = await _validator.ValidateSuperAdminRole(token);
            if (!validationResult.OK) return ServiceResult<List<AdminResponse>>.Fail(validationResult.ErrorMessage, validationResult.StatusCode);
            if (validationResult.Data == false) return ServiceResult<List<AdminResponse>>.Fail("User not a super admin.", 401);

            var admins = await _adminRepo.GetAllAdminsAsync(search, limit, page, sort);
            List<AdminResponse> mappedAdmins = new List<AdminResponse>();
            foreach (var admin in admins)
            {
                User matchedUser = await _userRepo.GetUserByIdAsync(admin.UserId);
                if (matchedUser == null) return ServiceResult<List<AdminResponse>>.Fail("No user found.", 404);
                AdminResponse mapped = _adminMapper.ToAdminLoginResponse(matchedUser, admin);
                mappedAdmins.Add(mapped);
            }

            return ServiceResult<List<AdminResponse>>.Success(mappedAdmins, 200);
        }
    }
}
