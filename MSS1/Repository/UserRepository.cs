using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSS1.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ITDbContext _context;

        public UserRepository(ITDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to be added.</param>
        /// <returns>The added user entity.</returns>
        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user); // Add the base User entity
            await _context.SaveChangesAsync();   // Save changes to the database
            return user; // Return the newly added User entity
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student); // Add the Student entity
            await _context.SaveChangesAsync();         // Save changes to the database
            return student;
        }

        public async Task<Lecturer> AddLecturerAsync(Lecturer lecturer)
        {
            await _context.Lecturers.AddAsync(lecturer); // Add the Lecturer entity
            await _context.SaveChangesAsync();           // Save changes to the database
            return lecturer;

        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)        // Include the Role entity for each user
                .Include(u => u.Student)     // Include the Student entity for users with role Student
                .Include(u => u.Lecturer)    // Include the Lecturer entity for users with role Lecturer
                .ToListAsync();              // Fetch the results asynchronously
        }





        /// <summary>
        /// Checks if a NIC already exists in the database.
        /// </summary>
        /// <param name="nicNumber">The NIC number to be checked.</param>
        /// <returns>True if the NIC exists; otherwise, false.</returns>
        public async Task<bool> IsNICExistsAsync(string nicNumber)
        {
            return await _context.Users.AnyAsync(u => u.NICNumber == nicNumber);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }


        /// <summary>
        /// Checks if an email already exists in the database.
        /// </summary>
        /// <param name="email">The email to be checked.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>


        /// <summary>
        /// Gets a user by their ID, including related entities.
        /// </summary>
        /// <param name="userId">The user ID to fetch the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        //public async Task<User> GetUserByIdAsync(int userId)
        //{
        //    return await _context.Users
        //        .Include(u => u.Role)       // Include Role entity
        //        .Include(u => u.Student)    // Include Student entity (if applicable)
        //        .Include(u => u.Lecturer)   // Include Lecturer entity (if applicable)
        //        .FirstOrDefaultAsync(u => u.UserId == userId); // Fetch user by ID
        //}
    }
}
