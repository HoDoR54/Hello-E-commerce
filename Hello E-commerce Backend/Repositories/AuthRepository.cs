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
        public async Task<Admin?> GetAdminByUserIdAsync(Guid Id)
        {
            return await _context.Admins
                                 .Include(a => a.User)
                                 .FirstOrDefaultAsync(a => a.User.Id == Id);
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(Guid Id)
        {
            return await _context.Customers
                                .Include(a => a.User)
                                .FirstOrDefaultAsync(a => a.User.Id == Id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
