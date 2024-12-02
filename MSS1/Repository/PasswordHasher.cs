using System.Text;
using Microsoft.AspNetCore.Identity;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;


    public class PasswordHasher : IPasswordHasher
    {
        // Hash a plaintext password and return both the hash and the salt
        public (string Hash, string Salt) HashPassword(string password)
        {
            // Hash the password
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Generate a salt (you can use any random generator for salt)
                byte[] salt = new byte[16]; // example size for salt
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                // Combine hash and salt
                var hashWithSalt = hashBytes.Concat(salt).ToArray();

                // Encode hash and salt in Base64
                return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(salt));
            }
        }

        // Verify a plaintext password against a hashed password and salt
        public bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = Convert.FromBase64String(storedSalt);

            // Recreate the hash for the input password
            using (var sha256 = SHA256.Create())
            {
                byte[] inputPasswordBytes = Encoding.UTF8.GetBytes(inputPassword);
                byte[] inputHash = sha256.ComputeHash(inputPasswordBytes.Concat(salt).ToArray());

                // Compare the hashes
                return inputHash.SequenceEqual(hashBytes);
            }
        }
    }

}
