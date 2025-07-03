using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IValidationService _validator;
        private readonly IAdminMapper _adminMapper;
        private readonly IUserRepository _userRepo;
        private readonly HttpContextAccessor _httpContextAccessor;

        public AdminService(IAdminRepository adminRepo, IValidationService validationService, IAdminMapper adminMapper, IUserRepository userRepo)
        {
            _adminRepo = adminRepo;
            _validator = validationService;
            _adminMapper = adminMapper;
            _userRepo = userRepo;
        }

        public async Task<ServiceResult<AdminResponse>> CreateNewAdminAsync(CreateAdminRequest req)
        {
            // validate credentials
            var credentialsValidationResult = _validator.ValidateCreateAdminRequest(req);
            if (!credentialsValidationResult.OK)
                return ServiceResult<AdminResponse>.Fail(credentialsValidationResult.ErrorMessage, credentialsValidationResult.StatusCode);

            var mappedUser = _adminMapper.CreateAdminRequestToUserModel(req);
            var user = await _userRepo.AddNewUserAsync(mappedUser);

            var httpContext = _httpContextAccessor.HttpContext;
            var userIdStr = httpContext?.User?.FindFirst("user_id")?.Value;

            if (!Guid.TryParse(userIdStr, out Guid superAdminId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID.");
            }

            var superAdmin = await _adminRepo.GetAdminByIdAsync(superAdminId);
            if (superAdmin == null)
            {
                throw new UnauthorizedAccessException("Super admin not found.");
            }

            var mappedAdmin = _adminMapper.CreateAdminRequestToAdminModel(req, user, superAdmin);

            var admin = await _adminRepo.AddNewAdminAsync(mappedAdmin);

            var response = _adminMapper.ToAdminResponse(user, admin);
            return ServiceResult<AdminResponse>.Success(response, 201);
        }

        public async Task<ServiceResult<AdminResponse>> DeleteAdminByIdAsync(Guid id)
        {
            var deletedAdmin = await _adminRepo.DeleteAdminByIdAsync(id);
            if (deletedAdmin == null) return ServiceResult<AdminResponse>.Fail("Deleting the admin failed.", 500);

            var matchedUser = await _userRepo.GetUserByIdAsync(deletedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No matched user found.", 404);

            var response = _adminMapper.ToAdminResponse(matchedUser, deletedAdmin);
            return ServiceResult<AdminResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<AdminResponse>> DemoteAdminAsync(Guid id)
        {
            if (id == Guid.Empty) return ServiceResult<AdminResponse>.Fail("Empty request.", 400);

            var demotedAdmin = await _adminRepo.DemoteAdminAsync(id);
            if (demotedAdmin == null) return ServiceResult<AdminResponse>.Fail("Demoting the admin failed.", 500);

            var matchedUser = await _userRepo.GetUserByIdAsync(demotedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No user found.", 404);

            var response = _adminMapper.ToAdminResponse(matchedUser, demotedAdmin);
            return ServiceResult<AdminResponse>.Success(response, 200);
        }

        public async Task<ServiceResult<AdminResponse>> GetAdminByIdAsync(Guid id)
        {
            var matchedAdmin = await _adminRepo.GetAdminByIdAsync(id);
            if (matchedAdmin == null) return ServiceResult<AdminResponse>.Fail("No admin record found.", 404);

            var matchedUser = await _userRepo.GetUserByIdAsync(matchedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No user record found.", 404);

            var mappedResponse = _adminMapper.ToAdminResponse(matchedUser, matchedAdmin);
            return ServiceResult<AdminResponse>.Success(mappedResponse, 200);
        }

        public async Task<ServiceResult<List<AdminResponse>>> GetAllAdminsAsync(string? search, int limit, int page, string? sort)
        {
            var admins = await _adminRepo.GetAllAdminsAsync(search, limit, page, sort);
            List<AdminResponse> mappedAdmins = new List<AdminResponse>();

            foreach (var admin in admins)
            {
                User? matchedUser = await _userRepo.GetUserByIdAsync(admin.UserId);
                if (matchedUser == null) return ServiceResult<List<AdminResponse>>.Fail("No user found.", 404);

                AdminResponse mapped = _adminMapper.ToAdminResponse(matchedUser, admin);
                mappedAdmins.Add(mapped);
            }

            return ServiceResult<List<AdminResponse>>.Success(mappedAdmins, 200);
        }

        public async Task<ServiceResult<AdminResponse>> PromoteAdminAsync(Guid id)
        {
            if (id == Guid.Empty) return ServiceResult<AdminResponse>.Fail("Empty request.", 400);

            var promotedAdmin = await _adminRepo.PromoteAdminAsync(id);
            if (promotedAdmin == null) return ServiceResult<AdminResponse>.Fail("Promoting the admin failed.", 500);

            var matchedUser = await _userRepo.GetUserByIdAsync(promotedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No user found.", 404);

            var response = _adminMapper.ToAdminResponse(matchedUser, promotedAdmin);
            return ServiceResult<AdminResponse>.Success(response, 200);
        }
    }
}
