namespace E_commerce_Admin_Dashboard.DTO.Requests.Auth
{
    public class UpdateEmailRequest
    {
        public Guid Id { get; set; }
        public string NewEmail { get; set; }
        public string Password { get; set; }
    }
}
