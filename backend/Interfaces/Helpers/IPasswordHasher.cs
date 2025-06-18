namespace E_commerce_Admin_Dashboard.Interfaces.Helpers
{
    public interface IPasswordHasher
    {
        bool Verify(string plainPassword, string hashedPassword);
        string Hash(string plainPassword);
    }

}
