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
        public async Task<bool> IsNICExistsAsync(string nicNumber)
        {
            return await _context.Users.AnyAsync(u => u.NICNumber == nicNumber);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Validate the email
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            // Load all users into memory and then filter (client-side evaluation)
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Student)
                .Include(u => u.Lecturer)
                .ToListAsync();

            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<User> GetUserByEmailLoginAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)  // Include Role entity
                .FirstOrDefaultAsync(u => u.Email == email);
        }


    }
}
