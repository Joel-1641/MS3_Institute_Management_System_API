using MSS1.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MSS1;

namespace MSS1 // Changed the namespace to MSS1 to match the folder
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(User user, string secretKey)
        {
            // Validate the secret key length
            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 16)
            {
                throw new ArgumentException("The secret key must be at least 16 characters long (128 bits).", nameof(secretKey));
            }

            // Create a symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Generate signing credentials using the key and HS256 algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            // Return the serialized JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
