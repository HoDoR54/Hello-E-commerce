using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
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
        private readonly IJwtHelper _jwtHelper;
        public AdminService(IJwtHelper jwtHelper, IAdminRepository adminRepo, IValidationService validationService, IAdminMapper adminMapper, IUserRepository userRepo)
        {
            _adminRepo = adminRepo;
            _validator = validationService;
            _adminMapper = adminMapper;
            _userRepo = userRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<ServiceResult<AdminResponse>> CreateNewAdminAsync(string token, CreateAdminRequest req)
        {
            // validate role
            Guid requestUserId = _jwtHelper.GetUserIdByToken(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<AdminResponse>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

            // validate credentials
            var credentialsValidationResult = _validator.ValidateCreateAdminRequest(req);
            if (!credentialsValidationResult.OK) return ServiceResult<AdminResponse>.Fail(credentialsValidationResult.ErrorMessage, credentialsValidationResult.StatusCode);

            var mappedUser = _adminMapper.CreateAdminRequestToUserModel(req);
            var user = await _userRepo.AddNewUserAsync(mappedUser);

            var mappedAdmin = _adminMapper.CreateAdminRequestToAdminModel(req, user, roleValidationResult.Data);
            var admin = await _adminRepo.AddNewAdminAsync(mappedAdmin);

            var response = _adminMapper.ToAdminLoginResponse(user, admin);
            return ServiceResult<AdminResponse>.Success(response, 201);
        }

        public async Task<ServiceResult<AdminResponse>> GetAdminByIdAsync(string token, Guid id)
        {
            // validate role
            Guid requestUserId = _jwtHelper.GetUserIdByToken(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<AdminResponse>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

            var matchedAdmin = await _adminRepo.GetAdminByIdAsync(id);
            if (matchedAdmin == null) return ServiceResult<AdminResponse>.Fail("No admin record found.", 404);

            var matchedUser = await _userRepo.GetUserByIdAsync(matchedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No user record found.", 404);

            var mappedResponse = _adminMapper.ToAdminLoginResponse(matchedUser, matchedAdmin);
            return ServiceResult<AdminResponse>.Success(mappedResponse, 200);
        }

        public async Task<ServiceResult<List<AdminResponse>>> GetAllAdminsAsync(string token ,string? search, int limit, int page, string? sort)
        {
            Guid requestUserId = _jwtHelper.GetUserIdByToken(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<List<AdminResponse>>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

            var admins = await _adminRepo.GetAllAdminsAsync(search, limit, page, sort);
            List<AdminResponse> mappedAdmins = new List<AdminResponse>();
            foreach (var admin in admins)
            {
                User? matchedUser = await _userRepo.GetUserByIdAsync(admin.UserId);
                if (matchedUser == null) return ServiceResult<List<AdminResponse>>.Fail("No user found.", 404);
                AdminResponse mapped = _adminMapper.ToAdminLoginResponse(matchedUser, admin);
                mappedAdmins.Add(mapped);
            }

            return ServiceResult<List<AdminResponse>>.Success(mappedAdmins, 200);
        }
    }
}
