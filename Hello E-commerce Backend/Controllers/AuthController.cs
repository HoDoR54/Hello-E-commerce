using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Helpers;
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
            ServiceResult<AdminLoginResponse> response = await _authServices.AdminLoginAsync(req);

            if (!response.OK)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("/customers/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequest req)
        {
            ServiceResult<CustomerLoginResponse> response = await _authServices.CustomerLoginAsync(req);

            if (!response.OK)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("/customers/register")]
        public async Task<IActionResult> CustomerRegister([FromBody] CustomerRegisterRequest req)
        {
            ServiceResult<CustomerRegisterResponse> response = await _authServices.CustomerRegisterAsync(req);

            if (!response.OK)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
