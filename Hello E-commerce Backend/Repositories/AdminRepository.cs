using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_Admin_Dashboard.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> AddNewAdminAsync(Admin admin)
        {
            var entry = await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Admin?> GetAdminByIdAsync(Guid id)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Admin?> GetAdminByUserIdAsync(Guid userId)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<List<Admin>> GetAllAdminsAsync(string? search, int limit, int page, string? sort)
        {
            var query = _context.Admins.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.Contains(search));
            }

            query = sort?.ToLower() switch
            {
                "name" => query.OrderBy(a => a.Name),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };

            int skip = (page - 1) * limit;
            query = query.Skip(skip).Take(limit);

            return await query.ToListAsync();
        }

    }
}
