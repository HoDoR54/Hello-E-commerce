using E_commerce_Admin_Dashboard.Interfaces.Helpers;

public class CookiesHelper : ICookiesHelper
{
    private CookieOptions GetCookieOptions(TimeSpan maxAge) => new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        MaxAge = maxAge,
        IsEssential = true
    };

    public void SetAccessTokenCookies(HttpResponse response, string accessToken)
    {
        response.Cookies.Append("access_token", accessToken, GetCookieOptions(TimeSpan.FromMinutes(15)));
    }

    public void SetRefreshTokenCookies(HttpResponse response, string refreshToken)
    {
        response.Cookies.Append("refresh_token", refreshToken, GetCookieOptions(TimeSpan.FromDays(7)));
    }

    public void ClearTokenCookies(HttpResponse response)
    {
        response.Cookies.Delete("access_token");
        response.Cookies.Delete("refresh_token");
    }
}
