using E_commerce_Admin_Dashboard.DTO.Requests;
using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Admin_Dashboard.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices; 
        private readonly ICookiesHelper _cookiesHelper;
        public AuthController (IAuthServices authServices, ICookiesHelper cookiesHelper)
        {
            _authServices = authServices;
            _cookiesHelper = cookiesHelper;
        }

        [HttpPost("/admins/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest req)
        {
            ServiceResult<AdminLoginResponse> response = await _authServices.AdminLoginAsync(req);

            if (!response.OK)
                return StatusCode(response.StatusCode, response);

            ServiceResult<TokenPair> tokenPair = await _authServices.GetTokens(req.Email);

            if (!tokenPair.OK)
                return StatusCode(tokenPair.StatusCode, tokenPair);

            _cookiesHelper.SetRefreshTokenCookies(HttpContext.Response, tokenPair.Data.RefreshToken);
            _cookiesHelper.SetAccessTokenCookies(HttpContext.Response, tokenPair.Data.AccessToken);

            return Ok(response);
        }

        [HttpPost("/customers/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequest req)
        {
            ServiceResult<CustomerLoginResponse> response = await _authServices.CustomerLoginAsync(req);

            if (!response.OK)
                return StatusCode(response.StatusCode, response);

            ServiceResult<TokenPair> tokenPair = await _authServices.GetTokens(req.Email);

            if (!tokenPair.OK)
                return StatusCode(tokenPair.StatusCode, tokenPair);

            _cookiesHelper.SetRefreshTokenCookies(HttpContext.Response, tokenPair.Data.RefreshToken);
            _cookiesHelper.SetAccessTokenCookies(HttpContext.Response, tokenPair.Data.AccessToken);

            return Ok(response);
        }

        [HttpPost("/customers/register")]
        public async Task<IActionResult> CustomerRegister([FromBody] CustomerRegisterRequest req)
        {
            ServiceResult<CustomerRegisterResponse> response = await _authServices.CustomerRegisterAsync(req);

            if (!response.OK)
                return StatusCode(response.StatusCode, response);

            ServiceResult<TokenPair> tokenPair = await _authServices.GetTokens(req.Email);

            if (!tokenPair.OK)
                return StatusCode(tokenPair.StatusCode, tokenPair);

            _cookiesHelper.SetRefreshTokenCookies(HttpContext.Response, tokenPair.Data.RefreshToken);
            _cookiesHelper.SetAccessTokenCookies(HttpContext.Response, tokenPair.Data.AccessToken);

            return Ok(response);
        }
    }
}
