using E_commerce_Admin_Dashboard.DTO.Requests.Admins;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.MicrosoftExtensions;

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
        public async Task<IActionResult> GetAllAdmin(
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
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest req)
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
        public async Task<IActionResult> GetAdminById([FromRoute] Guid id)
        {
            var token = Request.Cookies["access_token"];
            if (token == null) return Unauthorized("Token missing.");

            var serviceResponse = await _adminService.GetAdminByIdAsync(token, id);
            if (!serviceResponse.OK)
                return StatusCode(serviceResponse.StatusCode, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdminDetailsAsync([FromBody] UpdateAdminDetailsRequest req)
        {
            var accessToken = Request.Cookies["access_token"];
            if (accessToken == null) return Unauthorized("Token missing.");

            var serviceResult = await _adminService.UpdateAdminDetailsAsync(accessToken, req);
            return Ok();
        }
    }
}
