using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;
using System.Data;

namespace MSS1.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ITDbContext _context;

        public AuthenticationRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Authentication)
                .FirstOrDefaultAsync(u => u.Authentication.Email == email);
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            //if (role == null)
            //{
            //    Console.WriteLine($"Role with ID {roleId} not found."); // Debugging
            //}
            //return role;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Authentication> GetAuthenticationByEmailAsync(string email)
        {
            return await _context.Authentications
                .Include(a => a.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

    }
}
