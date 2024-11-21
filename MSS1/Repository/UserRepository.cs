using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ITDbContext _context;

        public UserRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Ensure that you're including the necessary related data using Include
            return await _context.Users
                .Include(u => u.Role)       // Include Role data
                .Include(u => u.Admin)      // Include Admin data (if the user is an Admin)
                .Include(u => u.Student)    // Include Student data (if the user is a Student)
                .ToListAsync();             // Asynchronously fetch the list
        }
        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Authentications.AnyAsync(auth => auth.Email == email);
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Authentication)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> IsEmailExistsAsyncById(string email, int excludeUserId)
        {
            return await _context.Authentications
                .AnyAsync(auth => auth.Email == email && auth.UserId != excludeUserId);
        }
    }
}
