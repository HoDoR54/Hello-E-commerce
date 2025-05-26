namespace E_commerce_Admin_Dashboard.Interfaces
{
    public interface ICookiesHelper
    {
        void SetAccessTokenCookies(HttpResponse response, string accessToken);
        void SetRefreshTokenCookies(HttpResponse response, string refreshToken);
    }
}
