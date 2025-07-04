using Azure.Core;
using E_commerce_Admin_Dashboard.DTO.Requests.Auth;
using E_commerce_Admin_Dashboard.DTO.Requests.Customers;
using E_commerce_Admin_Dashboard.DTO.Responses.Auth;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Admin_Dashboard.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authServices;
        private readonly ICookiesHelper _cookiesHelper;

        public AuthController(IAuthService authServices, ICookiesHelper cookiesHelper)
        {
            _authServices = authServices;
            _cookiesHelper = cookiesHelper;
        }

        [HttpPost("admins/login")]
        public async Task<IActionResult> AdminLoginAsync([FromBody] LoginRequest request)
        {
            var loginResult = await _authServices.LoginAdminAsync(request);

            if (loginResult.OK)
            {
                var tokenResult = await _authServices.GenerateTokenPairAsync(loginResult.Data.User.Email);
                if (!tokenResult.OK)
                    return StatusCode(tokenResult.StatusCode, tokenResult);
                _cookiesHelper.SetRefreshTokenCookies(Response, tokenResult.Data.RefreshToken);
                _cookiesHelper.SetAccessTokenCookies(Response, tokenResult.Data.AccessToken);
            }

            return StatusCode(loginResult.StatusCode, loginResult);
        }

        [HttpPost("customers/login")]
        public async Task<IActionResult> CustomerLoginAsync([FromBody] LoginRequest request)
        {
            var loginResult = await _authServices.LoginCustomerAsync(request);
            if (loginResult.OK)
            {
                var tokenResult = await _authServices.GenerateTokenPairAsync(loginResult.Data.User.Email);
                if (!tokenResult.OK)
                    return StatusCode(tokenResult.StatusCode, tokenResult);

                _cookiesHelper.SetRefreshTokenCookies(Response, tokenResult.Data.RefreshToken);
                _cookiesHelper.SetAccessTokenCookies(Response, tokenResult.Data.AccessToken);
            }

            return StatusCode(loginResult.StatusCode, loginResult);
        }

        [HttpPost("customers/register")]
        public async Task<IActionResult> CustomerRegisterAsync([FromBody] CustomerRegisterRequest request)
        {
            var registerResult = await _authServices.RegisterCustomerAsync(request);
            if (registerResult.OK)
            {
                var tokenResult = await _authServices.GenerateTokenPairAsync(request.Email);
                if (!tokenResult.OK)
                    return StatusCode(tokenResult.StatusCode, tokenResult);

                _cookiesHelper.SetRefreshTokenCookies(Response, tokenResult.Data.RefreshToken);
                _cookiesHelper.SetAccessTokenCookies(Response, tokenResult.Data.AccessToken);
            }

            return StatusCode(registerResult.StatusCode, registerResult);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync()
        {
            var accessToken = Request.Cookies["access_token"];
            if (accessToken == null)
                return Unauthorized(ServiceResult<object>.Fail("Missing access token.", 401));

            var validationResult = await _authServices.ValidateTokenAsync(accessToken, TokenType.Access);
            if (!validationResult.OK)
                return StatusCode(validationResult.StatusCode, validationResult);

            var response = await _authServices.GetUserByTokenAsync(accessToken);
            if (!response.OK)
                return StatusCode(response.StatusCode, response);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessTokenAsync()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return StatusCode(401, ServiceResult<object?>.Fail("Missing refresh token.", 401));
            }

            var refreshCheck = await _authServices.ValidateTokenAsync(refreshToken, TokenType.Refresh);
            if (!refreshCheck.OK)
                return StatusCode(refreshCheck.StatusCode, refreshCheck);

            var newAccessTokenResult = await _authServices.RefreshAccessToken(refreshCheck.Data);
            if (!newAccessTokenResult.OK)
                return StatusCode(newAccessTokenResult.StatusCode, newAccessTokenResult);

            _cookiesHelper.SetAccessTokenCookies(Response, newAccessTokenResult.Data);

            return StatusCode(200, ServiceResult<string>.Success("Access token refreshed.", 200));
        }
    }
}
