using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.MicrosoftExtensions;
using System.Threading.Tasks;

namespace E_commerce_Admin_Dashboard.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Get all admins (can be done by super admins only)
        [HttpGet]
        public async Task<IActionResult> GetAllAdminAsync(
            [FromQuery] string? search,
            [FromQuery] int limit = 10,
            [FromQuery] int page = 1,
            [FromQuery] string? sort = "createdAt")
        {
            var token = Request.Cookies["access_token"];

            if (token == null) return Unauthorized("Token missing.");

            var serviceResponse = await _adminService.GetAllAdminsAsync(token, search, limit, page, sort);

            if (!serviceResponse.OK)
                return StatusCode(serviceResponse.StatusCode, serviceResponse);

            return Ok(serviceResponse);
        }

        // Create a new admin
        [HttpPost]
        public async Task<IActionResult> CreateAdminAsync([FromBody] CreateAdminRequest req)
        {
            var token = Request.Cookies["access_token"];
            if (token == null) return Unauthorized("Token missing.");

            var serviceResponse = await _adminService.CreateNewAdminAsync(token, req);

            if (!serviceResponse.OK)
                return StatusCode(serviceResponse.StatusCode, serviceResponse);

            return Ok(serviceResponse);
        }

        // Get admin details by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminByIdAsync([FromRoute] Guid id)
        {
            var token = Request.Cookies["access_token"];
            if (token == null) return Unauthorized("Token missing.");

            var serviceResponse = await _adminService.GetAdminByIdAsync(token, id);
            if (!serviceResponse.OK)
                return StatusCode(serviceResponse.StatusCode, serviceResponse);

            return Ok(serviceResponse);
        }

        // Update admin details
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAdminDetailsAsync([FromBody] UpdateAdminDetailsRequest req, [FromRoute] Guid id)
        {
            var serviceResult = await _adminService.UpdateAdminDetailsAsync(id, req);
            if (!serviceResult.OK) return StatusCode(serviceResult.StatusCode, serviceResult);

            return Ok(serviceResult);
        }

        // Delete admin by Id
        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> DeleteAdminByIdAsync([FromRoute] Guid id)
        {
            var serviceResult = await _adminService.DeleteAdminByIdAsync(id);
            if (!serviceResult.OK) return StatusCode(serviceResult.StatusCode, null);

            return Ok(serviceResult);
        }

        // Promote the admin to super admin
        [HttpPatch("promote/{id}")]
        public async Task<IActionResult> PromoteToSuperAdminAsync([FromRoute] Guid id)
        {
            var token = Request.Cookies["access_token"];
            if (token == null) return Unauthorized("Missing token.");

            var serviceResult = await _adminService.PromoteAdminAsync(token, id);
            if (!serviceResult.OK) return StatusCode(serviceResult.StatusCode, serviceResult);

            return Ok(serviceResult);
        }

        [HttpPatch("demote/{id}")]
        public async Task<IActionResult> DemoteFromSuperAdminAsync([FromRoute] Guid id)
        {
            var token = Request.Cookies["access_token"];
            if (token == null) return Unauthorized("Missing token.");

            var serviceResult = await _adminService.DemoteAdminAsync(token, id);
            if (!serviceResult.OK) return StatusCode(serviceResult.StatusCode, serviceResult);

            return Ok(serviceResult);
        }
    }
}
