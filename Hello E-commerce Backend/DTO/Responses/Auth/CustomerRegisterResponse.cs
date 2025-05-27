namespace E_commerce_Admin_Dashboard.DTO.Responses.Auth
{
    public class CustomerRegisterResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CustomerAddressResponse CustomerAddress { get; set; }
    }
}
