using E_commerce_Admin_Dashboard.Interfaces.Repos;
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

        public async Task<RefreshToken> AddNewRefreshToken(RefreshToken token)
        {
            var entry = await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Admin?> GetAdminByUserIdAsync(Guid id)
        {
            return await _context.Admins
                                 .Include(a => a.User)
                                 .FirstOrDefaultAsync(a => a.User.Id == id);
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(Guid id)
        {
            return await _context.Customers
                                .Include(a => a.User)
                                .FirstOrDefaultAsync(a => a.User.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
