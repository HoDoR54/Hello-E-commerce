using E_commerce_Admin_Dashboard.Interfaces.Helpers;

namespace E_commerce_Admin_Dashboard.Helpers
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public bool Verify(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        public string Hash(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }
    }
}
