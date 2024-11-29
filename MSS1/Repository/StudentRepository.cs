using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ITDbContext _context;

        public StudentRepository(ITDbContext context)
        {
            _context = context;
        }
        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }


        // Get All Students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.User) // Include User details
                .ToListAsync();
        }

        // Get Student By Id
        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.User) // Include User details
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        // Delete Student By Id
        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student); // Remove Student entity
                _context.Users.Remove(student.User); // Remove associated User entity
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Student not found.");
            }
        }

        // Update Student
        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var existingStudent = await GetStudentByIdAsync(student.StudentId);
            if (existingStudent == null)
                throw new ArgumentException("Student not found.");

            // Update User fields
            existingStudent.User.FullName = student.User.FullName;
            existingStudent.User.Email = student.User.Email;
            existingStudent.User.Address = student.User.Address;
            existingStudent.User.MobileNumber = student.User.MobileNumber;
            existingStudent.User.Gender = student.User.Gender;

            // Update Student-specific fields
            existingStudent.RegistrationFee = student.RegistrationFee;
            existingStudent.IsRegistrationFeePaid = student.IsRegistrationFeePaid;

            await _context.SaveChangesAsync();
            return existingStudent;
        }
    }
}
