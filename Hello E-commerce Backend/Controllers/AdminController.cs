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

            var serviceResponse = await _adminService.GetAllAdmins(token, search, limit, page, sort);

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

            var serviceResponse = await _adminService.CreateNewAdmin(token, req);

            if (!serviceResponse.OK)
                return StatusCode(serviceResponse.StatusCode, serviceResponse);

            return Ok(serviceResponse);
        }
    }
}
