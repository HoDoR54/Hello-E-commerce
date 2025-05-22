using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("/admins/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest req)
        {
            var response = await _authServices.AdminLoginAsync(req);

            if (response == null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }

        [HttpPost("/customers/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequest req)
        {
            var response = await _authServices.CustomerLoginAsync(req);

            if (response == null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }
    }
}
