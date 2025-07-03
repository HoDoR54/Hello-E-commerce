using E_commerce_Admin_Dashboard.DTO.Responses.Admins;
using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public async Task<Admin?> DeleteAdminByIdAsync(Guid id)
        {
            var matchedAdmin = await _context.Admins.FindAsync(id);
            if (matchedAdmin == null) return null;
            matchedAdmin.IsDeleted = true;            
            await _context.SaveChangesAsync();
            return await _context.Admins.FindAsync(id);
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
                _ => query
            };


            int skip = (page - 1) * limit;
            query = query.Skip(skip).Take(limit);

            return await query.ToListAsync();
        }

        public async Task<string?> UpdateNameAsync(Guid id, string name)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
            if (admin == null) return null;

            admin.Name = name;
            await _context.SaveChangesAsync();
            return admin.Name;
        }

        public async Task<string?> UpdatePhoneNumAsync(Guid id, string phoneNumber)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
            if (admin == null) return null;

            var matchedUser = await _context.Users.FirstOrDefaultAsync(a => a.Id == admin.UserId);
            if (matchedUser == null) return null;

            matchedUser.PhoneNumber = phoneNumber;
            await _context.SaveChangesAsync();
            return matchedUser.PhoneNumber;
        }

        public async Task<Admin?> PromoteAdminAsync (Guid id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return null;

            if (admin.IsSuperAdmin) return null;
            admin.IsSuperAdmin = true;
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin?> DemoteAdminAsync(Guid id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return null;

            if (!admin.IsSuperAdmin) return null;
            admin.IsSuperAdmin = false;
            await _context.SaveChangesAsync();
            return admin;
        }
    }
}
