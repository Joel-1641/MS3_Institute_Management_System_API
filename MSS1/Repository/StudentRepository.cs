using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class StudentRepository: IStudentRepository
    {
        private readonly ITDbContext _context;

        public StudentRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            return await _context.Students
                .Include(s => s.User) // Include User if needed
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<Student> GetStudentProfile(int studentId)
        {
            return await _context.Students
                .Include(s => s.User) // Include related User entity
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }
        public async Task UpdateStudentProfile(int studentId, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(studentId);
            if (existingStudent == null) throw new Exception("Student not found");

            existingStudent.ProfilePicture = student.ProfilePicture;
            existingStudent.User.FullName = student.User.FullName;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetStudentCourses(int studentId)
        {
            return await _context.Courses
                .Where(c => c.Students.Any(s => s.StudentId == studentId))
                .ToListAsync();
        }

        //public async Task EnrollInCourse(int studentId, int courseId)
        //{
        //    var enrollment = new StudentCourse { StudentId = studentId, CourseId = courseId };
        //    _context.Add(enrollment);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<List<Payment>> GetStudentPayments(int studentId)
        {
            return await _context.Payments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
        public async Task EnrollInCourse(int studentId, int courseId)
        {
            // Check if the student is already enrolled in the course
            var existingEnrollment = await _context.Set<StudentCourse>()
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (existingEnrollment != null)
            {
                throw new InvalidOperationException("Student is already enrolled in the course.");
            }

            // If no existing enrollment, add a new record
            var enrollment = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.Set<StudentCourse>().Add(enrollment);
            await _context.SaveChangesAsync();
        }
        public async Task<Student> GetStudentWithCoursesAndPaymentsAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.User)  // Include the student's user data
                .Include(s => s.Courses)  // Include the student's courses
                .Include(s => s.Payments)  // Include the student's payment details
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }




    }
}
