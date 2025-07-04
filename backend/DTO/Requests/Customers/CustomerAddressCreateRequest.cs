namespace E_commerce_Admin_Dashboard.DTO.Requests.Customers
{
    public class CustomerAddressCreateRequest
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
