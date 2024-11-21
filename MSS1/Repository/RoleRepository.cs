using MSS1.Entities;
using Microsoft.EntityFrameworkCore;
using MSS1.Interfaces;
using System;
using MSS1.Database;

namespace MSS1.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ITDbContext _context;

        public RoleRepository(ITDbContext context)
        {
            _context = context;
        }

        // Method to fetch a role by its ID
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(role => role.RoleId == roleId);
        }

        // Method to fetch all roles
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
