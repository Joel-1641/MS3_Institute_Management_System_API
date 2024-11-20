using System.Text;
using Microsoft.AspNetCore.Identity;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class PasswordHasher: IPasswordHasher
    {
        public (string hashedPassword, string salt) HashPassword(string password)
        {
            var salt = Guid.NewGuid().ToString();
            var hashedPassword = Convert.ToBase64String(
                new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(salt))
                .ComputeHash(Encoding.UTF8.GetBytes(password))
            );
            return (hashedPassword, salt);
        }

        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var computedHash = Convert.ToBase64String(
                new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(salt))
                .ComputeHash(Encoding.UTF8.GetBytes(password))
            );
            return computedHash == hashedPassword;
        }
    }
}
