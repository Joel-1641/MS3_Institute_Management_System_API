using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ITDbContext _ItDbContext;

    public UserRepository(ITDbContext itDbContext)
        {
            _ItDbContext = itDbContext;
        }
        public async Task<User> RegisterUser(User user, string password)
        {
            // Hash the password and create the Authentication record
            var salt = GenerateSalt(); // Implement a method to generate salt
            var hashedPassword = HashPassword(password, salt); // Implement a method to hash the password

            var authentication = new Authentication
            {
                Email = user.Authentication.Email,
                HashedPassword = hashedPassword,
                PasswordSalt = salt
            };

            user.Authentication = authentication;
            _ItDbContext.Users.Add(user);
            await _ItDbContext.SaveChangesAsync();

            return user;
        }
        public async Task<Role> GetRoleById(int roleId)
        {
            return await _ItDbContext.Roles.FindAsync(roleId);
        }

        private string GenerateSalt(int size = 16)
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[size];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        private string HashPassword(string password, string salt)
        {
            // Combine password and salt
            var passwordWithSalt = password + salt;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(passwordWithSalt);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
