using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace E_commerce_Admin_Dashboard.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        public AuthController (IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("/admin/login")]
        public async Task<IActionResult> AdminLogIn([FromBody] AdminLoginRequest req)
        {
            var response = await _authServices.AdminLoginAsync(req);

            if (response == null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }
    }
}
