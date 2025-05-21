using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Admin_Dashboard.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
