using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using MSS1.Repositories;
using MSS1.Repository;
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

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IStudentRepository studentRepository, ILecturerRepository lecturerRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
        }

        //public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        //{
        //    var users = await _userRepository.GetAllUsersAsync();

        //    // Map users to DTOs
        //    var userResponseDTOs = users.Select(user => new UserResponseDTO
        //    {
        //        UserId = user.UserId,
        //        FullName = user.FullName,
        //        RoleName = user.Role?.RoleName, // Get the RoleName from the Role entity
        //        ProfilePicture = user.Student?.ProfilePicture // Only applicable for Students
        //    });

        //    return userResponseDTOs;
        //}

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
                    throw new ArgumentException("Registration fee must be paid for students.");

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

        //public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        //{
        //    var users = await _userRepository.GetAllUsersAsync();

        //    // Map User entity to UserResponseDTO
        //    return users.Select(user => new UserResponseDTO
        //    {
        //        UserId = user.UserId,
        //        FullName = user.FullName,
        //        Email = user.Email,
        //        RoleName = user.Role?.RoleName,
        //        ProfilePicture = user.ProfilePicture, // Include Profile Picture
        //        AdditionalData = user.RoleId == 2 ? // If Student (RoleId == 2)
        //                         new { user.Student?.RegistrationFee, user.Student?.IsRegistrationFeePaid } :
        //                         user.RoleId == 3 ? // If Lecturer (RoleId == 3)
        //                         new { Courses = user.Lecturer?.Courses.Select(c => c.CourseName).ToList() } :
        //                         null
        //    });
        //}




        //public async Task<AddUserResponseDTO> UpdateUserAsync(int userId, AddUserRequestDTO request)
        //{
        //    // Fetch the existing user
        //    var user = await _userRepository.GetUserByIdAsync(userId);
        //    if (user == null) throw new KeyNotFoundException("User not found.");

        //    // Check if email is unique
        //    if (await _userRepository.IsEmailExistsAsyncById(request.Email, userId))
        //        throw new ArgumentException("Email is already in use.");

        //    // Update user properties
        //    user.FullName = request.FullName;
        //    user.RoleId = request.RoleId;

        //    // Update authentication details
        //    user.Authentication.Email = request.Email;

        //    if (!string.IsNullOrEmpty(request.Password))
        //    {
        //        var salt = GenerateSalt();
        //        user.Authentication.PasswordSalt = salt;
        //        user.Authentication.HashedPassword = HashPassword(request.Password, salt);
        //    }

        //    // Save changes
        //    var updatedUser = await _userRepository.UpdateUserAsync(user);

        //    // Get the role for the response
        //    var role = await _roleRepository.GetRoleByIdAsync(updatedUser.RoleId);

        //    return new AddUserResponseDTO
        //    {
        //        UserId = updatedUser.UserId,
        //        FullName = updatedUser.FullName,
        //        Email = updatedUser.Authentication.Email,
        //        RoleName = role.RoleName
        //    };
        //}
        //public async Task<AddUserResponseDTO> DeleteUserAsync(int userId)
        //{
        //    var userExists = await _userRepository.IsUserExistsAsync(userId);
        //    if (!userExists)
        //        throw new KeyNotFoundException("User not found.");

        //    var user = await _userRepository.GetUserByIdAsync(userId);

        //    // Handle the role and user deletion process
        //    await _userRepository.DeleteUserAsync(userId);

        //    // Return user data as a response after deletion
        //    return new AddUserResponseDTO
        //    {
        //        UserId = user.UserId,
        //        FullName = user.FullName,
        //        Email = user.Authentication?.Email,
        //        RoleName = user.Role?.RoleName
        //    };
        //}

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
        //public async Task AddStudentAsync(int userId, bool isRegistrationFeePaid)
        //{
        //    // Validate the registration fee
        //    if (!isRegistrationFeePaid)
        //    {
        //        throw new ArgumentException("Registration fee must be paid for the student to be created.");
        //    }

        //    // Check if the user exists
        //    var user = await _userRepository.GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        throw new KeyNotFoundException("User not found.");
        //    }

        //    // Create a new student record
        //    var student = new Student
        //    {
        //        UserId = userId,
        //        RegistrationFee = 10000.00m, // Example registration fee
        //        IsRegistrationFeePaid = isRegistrationFeePaid,
        //        ProfilePicture = null // Set default profile picture or leave null
        //    };

        //    await _studentRepository.AddStudentAsync(student);
        //}


    }
}
