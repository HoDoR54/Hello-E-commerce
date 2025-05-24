﻿using E_commerce_Admin_Dashboard.DTO.Responses;
using E_commerce_Admin_Dashboard.Interfaces;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Models;

namespace E_commerce_Admin_Dashboard.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddNewUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
