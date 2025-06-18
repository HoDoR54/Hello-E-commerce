namespace E_commerce_Admin_Dashboard.Interfaces.Helpers
{
    public interface ICookiesHelper
    {
        void SetAccessTokenCookies(HttpResponse response, string accessToken);
        void ClearTokenCookies(HttpResponse response);
        void SetRefreshTokenCookies(HttpResponse response, string refreshToken);
    }
}
