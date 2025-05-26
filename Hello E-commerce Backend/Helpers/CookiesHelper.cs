using E_commerce_Admin_Dashboard.Interfaces;
using Microsoft.Identity.Client;

namespace E_commerce_Admin_Dashboard.Helpers
{
    public class CookiesHelper : ICookiesHelper
    {
        public void SetAccessTokenCookies (HttpResponse response, string accessToken)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromMinutes(15),
                IsEssential = true
            };
            response.Cookies.Append("access_token", accessToken, options);
        }

        public void SetRefreshTokenCookies (HttpResponse response, string refreshToken)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(7),
                IsEssential = true
            };
            response.Cookies.Append("refresh_token", refreshToken, options);
        }
    }
}
