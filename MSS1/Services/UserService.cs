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

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IStudentRepository studentRepository, ILecturerRepository lecturerRepository, string jwtKey)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _jwtKey = jwtKey;
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
        //public async Task<StudentResponseDTO> GetStudentByNICAsync(string nicNumber)
        //{
        //    var student = await _studentRepository.GetStudentByNICAsync(nicNumber);
        //    if (student == null)
        //        throw new ArgumentException("Student not found with the given NIC.");

        //    return new StudentResponseDTO
        //    {
        //        FullName = student.User.FullName,
        //        Email = student.User.Email,
        //        Address = student.User.Address,
        //        MobileNumber = student.User.MobileNumber,
        //        Gender = student.User.Gender,
        //        NICNumber = student.User.NICNumber,
        //        DateOfBirth = student.User.DateOfBirth
        //    };
        //}

        public async Task<List<LecturerResponseDTO>> GetAllLecturersAsync()
        {
            var lecturers = await _lecturerRepository.GetAllLecturersAsync();

            return lecturers.Select(l => new LecturerResponseDTO
            {
                LecturerId = l.LecturerId,
                FullName = l.User.FullName,
                NICNumber = l.User.NICNumber,
                Email = l.User.Email,
                Gender = l.User.Gender,
                Address = l.User.Address,
                MobileNumber = l.User.MobileNumber,
                DateOfBirth = l.User.DateOfBirth,
                ProfilePicture = l.User.ProfilePicture,
               // RoleName = l.User.Role?.RoleName, // Map the RoleName here
                Courses = l.Courses.Select(c => c.CourseName).ToList()
            }).ToList();
        }

        public async Task<LecturerResponseDTO> GetLecturerByIdAsync(int lecturerId)
        {
            var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
            if (lecturer == null) return null;

            return new LecturerResponseDTO
            {
                UserId = lecturer.User.UserId,
                FullName = lecturer.User.FullName,
                Email = lecturer.User.Email,
                //RoleName = "Lecturer",
                ProfilePicture = lecturer.User.ProfilePicture,
                LecturerId = lecturer.LecturerId,
                Courses = lecturer.Courses.Select(c => c.CourseName).ToList()
            };
        }


        public async Task DeleteLecturerAsync(int lecturerId)
        {
            await _lecturerRepository.DeleteLecturerAsync(lecturerId);
        }

        public async Task UpdateLecturerAsync(int lecturerId, UpdateLecturerRequestDTO request)
        {
            var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
            if (lecturer == null) throw new ArgumentException("Lecturer not found.");

            // Update User details
            lecturer.User.FullName = request.FullName;
            lecturer.User.Email = request.Email;
            lecturer.User.NICNumber = request.NICNumber;
            lecturer.User.Gender = request.Gender;
            lecturer.User.Address = request.Address;
            lecturer.User.MobileNumber = request.MobileNumber;
            lecturer.User.ProfilePicture = request.ProfilePicture;
            lecturer.User.DateOfBirth = request.DateOfBirth;

            // Update Courses
            lecturer.Courses = request.Courses.Select(course => new LecturerCourse { CourseName = course }).ToList();

            await _lecturerRepository.UpdateLecturerAsync(lecturer);
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
