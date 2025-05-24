using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.DTO.Requests
{
    public class CustomerRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CustomerAddressCreateRequest CustomerAddress { get; set; }
    }
}
