using Microsoft.IdentityModel.Tokens;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using MSS1.Repositories;
using MSS1.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MSS1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly string _jwtKey;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IStudentRepository studentRepository, ILecturerRepository lecturerRepository, string jwtKey, IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _jwtKey = jwtKey;
            _emailService = emailService;
        }


        public async Task<AddUserResponseDTO> AddUserAsync(AddUserRequestDTO request)
        {
            // Check for valid RoleId (Only RoleId = 2 or RoleId = 3 are allowed)
            if (request.RoleId != 2 && request.RoleId != 3)
                throw new ArgumentException("Invalid RoleId. Only RoleId 2 (Student) and RoleId 3 (Lecturer) are allowed.");

            // Check uniqueness of NIC and Email
            if (await _userRepository.IsNICExistsAsync(request.NICNumber))
                throw new ArgumentException("NIC number already exists.");
            if (await _userRepository.IsEmailExistsAsync(request.Email))
                throw new ArgumentException("Email already exists.");

            // Hash the password
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            // Create the base User entity
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                NICNumber = request.NICNumber,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                RoleId = request.RoleId,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Address = request.Address,
                MobileNumber = request.MobileNumber,
                ProfilePicture = request.ProfilePicture
            };

            // Save the User entity
            var addedUser = await _userRepository.AddUserAsync(user);

            // Handle role-specific data (Student or Lecturer)
            if (request is AddStudentRequestDTO studentRequest) // If it's a Student
            {
                if (!studentRequest.IsRegistrationFeePaid)
                    throw new ArgumentException("Registration fee must be paid by students.");

                var student = new Student
                {
                    UserId = addedUser.UserId,
                    RegistrationFee = studentRequest.RegistrationFee,
                    IsRegistrationFeePaid = studentRequest.IsRegistrationFeePaid
                };
                await _userRepository.AddStudentAsync(student);

                // Send email to the student
                var emailBody = $"Dear {addedUser.FullName} ({addedUser.NICNumber}),\n\n" +
                "Congratulations on your successful registration as a student. You are now able to select your courses at your convenience.\n\n" +
                "Should you have any questions or need assistance, feel free to reach out to us.\n\n" +
                "Best regards,\n" +
                "The ITScholar Team";
                await _emailService.SendEmailAsync(addedUser.Email, "Student Registration Successful", emailBody);
            }
            else if (request is AddLecturerRequestDTO lecturerRequest) // If it's a Lecturer
            {
                if (lecturerRequest.Courses == null || !lecturerRequest.Courses.Any())
                    throw new ArgumentException("Courses must be specified for lecturers.");

                var lecturer = new Lecturer
                {
                    UserId = addedUser.UserId,
                    Courses = lecturerRequest.Courses.Select(course => new LecturerCourse { CourseName = course }).ToList()
                };
                await _userRepository.AddLecturerAsync(lecturer);
            }

            // Get the Role name for the response
            var role = await _roleRepository.GetRoleByIdAsync(request.RoleId);

            // Return response DTO
            return new AddUserResponseDTO
            {
                UserId = addedUser.UserId,
                FullName = addedUser.FullName,
                Email = addedUser.Email,
                RoleName = role?.RoleName,
                ProfilePicture = addedUser.ProfilePicture
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
        private bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var hashedInputPassword = HashPassword(password, salt);
            return hashedInputPassword == hashedPassword;
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPassword));
            }
        }
        

       
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("Invalid email or password.");

            // Generate JWT Token
            var token = GenerateJwtToken(user);

            // Get the role name
            var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

            return new LoginResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                FullName = user.FullName,
                RoleName = role?.RoleName
            };
        }

        // Logout API - No specific implementation, client-side can handle token invalidation
        public Task LogoutAsync() => Task.CompletedTask;

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName),
                new Claim("UserId", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       

       


    }
}
