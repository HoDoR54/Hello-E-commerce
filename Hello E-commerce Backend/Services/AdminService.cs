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
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

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
            Guid requestUserId = _jwtHelper.GetUserIdByTokenAsync(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<AdminResponse>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

            // validate credentials
            var credentialsValidationResult = _validator.ValidateCreateAdminRequest(req);
            if (!credentialsValidationResult.OK) return ServiceResult<AdminResponse>.Fail(credentialsValidationResult.ErrorMessage, credentialsValidationResult.StatusCode);

            var mappedUser = _adminMapper.CreateAdminRequestToUserModel(req);
            var user = await _userRepo.AddNewUserAsync(mappedUser);

            var mappedAdmin = _adminMapper.CreateAdminRequestToAdminModel(req, user, roleValidationResult.Data);
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

        public async Task<ServiceResult<AdminResponse>> GetAdminByIdAsync(string token, Guid id)
        {
            // validate role
            Guid requestUserId = _jwtHelper.GetUserIdByTokenAsync(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<AdminResponse>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

            var matchedAdmin = await _adminRepo.GetAdminByIdAsync(id);
            if (matchedAdmin == null) return ServiceResult<AdminResponse>.Fail("No admin record found.", 404);

            var matchedUser = await _userRepo.GetUserByIdAsync(matchedAdmin.UserId);
            if (matchedUser == null) return ServiceResult<AdminResponse>.Fail("No user record found.", 404);

            var mappedResponse = _adminMapper.ToAdminResponse(matchedUser, matchedAdmin);
            return ServiceResult<AdminResponse>.Success(mappedResponse, 200);
        }

        public async Task<ServiceResult<List<AdminResponse>>> GetAllAdminsAsync(string token ,string? search, int limit, int page, string? sort)
        {
            Guid requestUserId = _jwtHelper.GetUserIdByTokenAsync(token);
            var roleValidationResult = await _validator.ValidateAndReturnSuperAdminAsync(requestUserId);
            if (!roleValidationResult.OK) return ServiceResult<List<AdminResponse>>.Fail(roleValidationResult.ErrorMessage, roleValidationResult.StatusCode);

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

        public async Task<ServiceResult<AdminResponse>> UpdateAdminDetailsAsync(Guid id, UpdateAdminDetailsRequest req)
        {
            if (req == null)
                return ServiceResult<AdminResponse>.Fail("Request is empty.", 400);

            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
                return ServiceResult<AdminResponse>.Fail("No user found.", 404);

            var admin = await _adminRepo.GetAdminByUserIdAsync(id);
            if (admin == null)
                return ServiceResult<AdminResponse>.Fail("No admin record found.", 404);

            string email = user.Email;
            string name = admin.Name;
            string phoneNumber = admin.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(req.Email) && req.Email != user.Email)
            {
                var updated = await _userRepo.UpdateEmailAsync(user.Id, req.Email);
                if (updated == null)
                    return ServiceResult<AdminResponse>.Fail("Failed to update email.", 500);
                email = updated;
            }

            if (!string.IsNullOrWhiteSpace(req.Name) && req.Name != admin.Name)
            {
                var updated = await _adminRepo.UpdateNameAsync(user.Id, req.Name);
                if (updated == null)
                    return ServiceResult<AdminResponse>.Fail("Failed to update name.", 500);
                name = req.Name;
            }

            if (!string.IsNullOrWhiteSpace(req.PhoneNumber) && req.PhoneNumber != admin.PhoneNumber)
            {
                var updated = await _adminRepo.UpdatePhoneNumAsync(user.Id, req.PhoneNumber);
                if (updated == null)
                    return ServiceResult<AdminResponse>.Fail("Failed to update phone number.", 500);
                phoneNumber = req.PhoneNumber;
            }

            var updatedUser = await _userRepo.GetUserByIdAsync(user.Id);
            var updatedAdmin = await _adminRepo.GetAdminByUserIdAsync(user.Id);

            var response = _adminMapper.ToAdminResponse(updatedUser, updatedAdmin);

            return ServiceResult<AdminResponse>.Success(response, 200);
        }
    }
}
