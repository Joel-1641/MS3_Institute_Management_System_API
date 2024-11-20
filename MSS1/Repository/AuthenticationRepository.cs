using Microsoft.EntityFrameworkCore; // Ensure proper EF Core namespace is included
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;
using System.Threading.Tasks;

namespace MSS1.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository // Corrected syntax for class declaration
    {
        private readonly ITDbContext _context; // Corrected initialization

        // Constructor to inject ITDbContext dependency
        public AuthenticationRepository(ITDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Null check added for robustness
        }

        /// <summary>
        /// Retrieves a user by their email address, including their authentication details.
        /// </summary>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Authentication)
                .FirstOrDefaultAsync(u => u.Authentication.Email == email); // Includes Authentication details
        }

        /// <summary>
        /// Retrieves a role by its ID.
        /// </summary>
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId); // Queries roles by roleId
        }

        /// <summary>
        /// Adds a new user to the database context.
        /// </summary>
        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user)); // Validation added
            await _context.Users.AddAsync(user); // Adds user asynchronously
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); // Saves all pending changes
        }

        /// <summary>
        /// Retrieves authentication details by email, including user and role.
        /// </summary>
        public async Task<Authentication> GetAuthenticationByEmailAsync(string email)
        {
            return await _context.Authentications
                .Include(a => a.User)
                .ThenInclude(u => u.Role) // Includes User and Role for deeper navigation
                .FirstOrDefaultAsync(a => a.Email == email); // Finds by email
        }

        /// <summary>
        /// Updates a user's password and clears their reset token.
        /// </summary>
        public async Task UpdatePasswordAsync(Authentication auth, string newHashedPassword, string newSalt)
        {
            if (auth == null) throw new ArgumentNullException(nameof(auth)); // Validation added

            auth.HashedPassword = newHashedPassword ?? throw new ArgumentNullException(nameof(newHashedPassword));
            auth.PasswordSalt = newSalt ?? throw new ArgumentNullException(nameof(newSalt));
            auth.PasswordResetToken = null; // Clears the reset token
            auth.TokenExpiration = null; // Clears the token expiration time
            await _context.SaveChangesAsync(); // Commits changes to the database
        }
       
        public async Task<Authentication> GetByResetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Reset token cannot be null or empty.", nameof(token));

            return await _context.Authentications
                .Include(a => a.User) // Includes related User entity if necessary
                .FirstOrDefaultAsync(a => a.PasswordResetToken == token && a.TokenExpiration >= DateTime.UtcNow);
        }

    }
}
