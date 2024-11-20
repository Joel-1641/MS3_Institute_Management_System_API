using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Helpers;
using MSS1.Interfaces;
using MSS1.Repository;
using System;
using System.Threading.Tasks;

namespace MSS1.Services
{
    public class AuthenticService : IAuthenticServices
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticService(IAuthenticationRepository repository, IPasswordHasher passwordHasher, ITokenRepository tokenRepository)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO requestDTO)
        {
            // Check if the user already exists
            var existingUser = await _repository.GetUserByEmailAsync(requestDTO.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            // Fetch role
            var role = await _repository.GetRoleByIdAsync(requestDTO.RoleId);
            if (role == null)
            {
                throw new Exception("Invalid role ID.");
            }

            // Hash the password
            var (hashedPassword, salt) = _passwordHasher.HashPassword(requestDTO.Password);

            // Create user and authentication objects
            var user = new User
            {
                FullName = requestDTO.FullName,
                RoleId = requestDTO.RoleId,
                Authentication = new Authentication
                {
                    Email = requestDTO.Email,
                    HashedPassword = hashedPassword,
                    PasswordSalt = salt
                }
            };

            // Add user to the database
            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();

            // Return response
            return new RegisterUserResponseDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Authentication.Email,
                RoleName = role.RoleName,
                Message = "User registered successfully."
            };
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO, string secretKey)
        {
            // Fetch authentication details by email
            var auth = await _repository.GetAuthenticationByEmailAsync(requestDTO.Email);
            if (auth == null)
            {
                throw new Exception("Invalid email or password.");
            }

            // Verify the password
            var isPasswordValid = _passwordHasher.VerifyPassword(requestDTO.Password, auth.HashedPassword, auth.PasswordSalt);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            // Generate JWT token
            var token = TokenHelper.GenerateJwtToken(auth.User, secretKey);

            // Prepare response
            return new LoginResponseDTO
            {
                Token = token,
                FullName = auth.User.FullName,
                Role = auth.User.Role.RoleName
            };

        }
        public async Task LogoutAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");

            // Example: Add token to blacklist or cache for invalidation
            await _tokenRepository.InvalidateTokenAsync(token);
        }
    }
}
