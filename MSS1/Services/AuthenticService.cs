using Microsoft.EntityFrameworkCore;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSS1.Services
{
    public class AuthenticService : IAuthenticServices
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository userRepository;

        public AuthenticService(
            IAuthenticationRepository repository,
            IPasswordHasher passwordHasher,
            ITokenRepository tokenRepository,
            IUserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO requestDTO)
        {
            if (requestDTO == null) throw new ArgumentNullException(nameof(requestDTO));

            if (requestDTO.RoleId != 1)
                throw new Exception("Only Admins are allowed to register.");

            var existingUser = await _repository.GetUserByEmailAsync(requestDTO.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            var role = await _repository.GetRoleByIdAsync(requestDTO.RoleId)
                ?? throw new Exception("Invalid role ID.");

            var (hashedPassword, salt) = _passwordHasher.HashPassword(requestDTO.Password);

            var user = new User
            {
                FullName = requestDTO.FullName,
                RoleId = requestDTO.RoleId,
                Email = requestDTO.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };

            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();

            var admin = new Admin
            {
                UserId = user.UserId,
                //AdminRoleType = "SuperAdmin", // Example default role type
               // AdminPhoneNumber = "N/A",
                //LastLoginDate = DateTime.UtcNow
            };

            await AddAdminAsync(admin);

            return new RegisterUserResponseDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                RoleName = role.RoleName,
                Message = "Admin registered successfully."
            };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO, string secretKey)
        {
            var user = await _repository.GetUserByEmailAsync(requestDTO.Email)
                ?? throw new Exception("Invalid email or password.");

            if (!_passwordHasher.VerifyPassword(requestDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid email or password.");

            var token = TokenHelper.GenerateJwtToken(user, secretKey);

            return new LoginResponseDTO
            {
                Token = token,
                FullName = user.FullName,
                Role = user.Role.RoleName
            };
        }
        public async Task<LoginResponseDTO> LoginAsyncUsers(LoginRequestDTO requestDTO, string secretKey)
        {
            var user = await _repository.GetUserByEmailAsync(requestDTO.Email)
                ?? throw new Exception("Invalid email or password.");

            if (!_passwordHasher.VerifyPassword(requestDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid email or password.");

            var token = TokenHelper.GenerateJwtToken(user, secretKey);

            var roleName = user.Role.RoleName;

            // Check roles and provide role-specific info
            if (roleName == "Student")
            {
                // Additional logic for students if needed
            }
            else if (roleName == "Lecturer")
            {
                // Additional logic for lecturers if needed
            }

            return new LoginResponseDTO
            {
                Token = token,
                FullName = user.FullName,
                Role = roleName
            };
        }

        public async Task LogoutAsyncUsers(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");

            await _tokenRepository.InvalidateTokenAsync(token);
        }

        

        public async Task LogoutAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");

            await _tokenRepository.InvalidateTokenAsync(token);
        }

        public async Task AddAdminAsync(Admin admin)
        {
            if (admin == null) throw new ArgumentNullException(nameof(admin));
            await _repository.AddAdminAsync(admin);
            await _repository.SaveChangesAsync();
        }

        public async Task ValidateEmailDomain(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required.");

            var emailParts = email.Split('@');
            if (emailParts.Length != 2 || emailParts[1].ToLower() != "gmail.com")
                throw new Exception("Only Gmail addresses are allowed.");
        }

        public async Task ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required.");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
                throw new Exception("Invalid email format.");

            var domain = email.Split('@').LastOrDefault();
            if (domain == null || domain.ToLower() != "gmail.com")
                throw new Exception("Only Gmail addresses are allowed.");
        }
       

         
    }
}
