using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using MSS1.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace MSS1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            // Map users to DTOs
            var userResponseDTOs = users.Select(user => new UserResponseDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                RoleName = user.Role?.RoleName, // Get the RoleName from the Role entity
                ProfilePicture = user.Student?.ProfilePicture // Only applicable for Students
            });

            return userResponseDTOs;
        }

        public async Task<AddUserResponseDTO> AddUserAsync(AddUserRequestDTO request)
        {
            // Check if the email already exists
            if (await _userRepository.IsEmailExistsAsync(request.Email))
                throw new ArgumentException("Email already exists.");

            // Hashing password
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            // Create User entity
            var user = new User
            {
                FullName = request.FullName,
                RoleId = request.RoleId,
                Authentication = new Authentication
                {
                    Email = request.Email,
                    HashedPassword = hashedPassword,
                    PasswordSalt = salt
                }
            };

            // Save the user
            var addedUser = await _userRepository.AddUserAsync(user);

            // Retrieve the associated role
            var role = await _roleRepository.GetRoleByIdAsync(addedUser.RoleId);

            // Map to response DTO
            return new AddUserResponseDTO
            {
                UserId = addedUser.UserId,
                FullName = addedUser.FullName,
                Email = addedUser.Authentication.Email,
                RoleName = role?.RoleName ?? "Unknown Role" // Null-check for role
            };
        }
        public async Task<AddUserResponseDTO> UpdateUserAsync(int userId, AddUserRequestDTO request)
        {
            // Fetch the existing user
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            // Check if email is unique
            if (await _userRepository.IsEmailExistsAsyncById(request.Email, userId))
                throw new ArgumentException("Email is already in use.");

            // Update user properties
            user.FullName = request.FullName;
            user.RoleId = request.RoleId;

            // Update authentication details
            user.Authentication.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Password))
            {
                var salt = GenerateSalt();
                user.Authentication.PasswordSalt = salt;
                user.Authentication.HashedPassword = HashPassword(request.Password, salt);
            }

            // Save changes
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            // Get the role for the response
            var role = await _roleRepository.GetRoleByIdAsync(updatedUser.RoleId);

            return new AddUserResponseDTO
            {
                UserId = updatedUser.UserId,
                FullName = updatedUser.FullName,
                Email = updatedUser.Authentication.Email,
                RoleName = role.RoleName
            };
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPassword));
            }
        }
    }
}
