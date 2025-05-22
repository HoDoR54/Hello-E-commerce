using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_commerce_Admin_Dashboard.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins
                                 .Include(a => a.User)
                                 .FirstOrDefaultAsync(a => a.User.Email == email);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                                .Include(a => a.User)
                                .FirstOrDefaultAsync(a => a.User.Email == email);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
