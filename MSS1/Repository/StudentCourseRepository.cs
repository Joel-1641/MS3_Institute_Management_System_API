using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly ITDbContext _context;

        public StudentCourseRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task AddStudentCoursesAsync(IEnumerable<StudentCourse> studentCourses)
        {
            foreach (var studentCourse in studentCourses)
            {
                var alreadyEnrolled = await _context.StudentCourses
                    .AnyAsync(sc => sc.StudentId == studentCourse.StudentId && sc.CourseId == studentCourse.CourseId);

                if (!alreadyEnrolled)
                {
                    await _context.StudentCourses.AddAsync(studentCourse);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<string> GetStudentNICByIdAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with ID {studentId}.");
            }

            return student.User.NICNumber;
        }

        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Select(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<(Student Student, IEnumerable<Course> Courses)>> GetAllStudentCoursesAsync()
        {
            var studentCourses = await _context.StudentCourses
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Course)
                .ToListAsync();

            return studentCourses
                .GroupBy(sc => sc.Student)
                .Select(group => (group.Key, group.Select(sc => sc.Course)));
        }

        public async Task<int> GetStudentCountForCourseAsync(int courseId)
        {
            return await _context.StudentCourses.CountAsync(sc => sc.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Student> GetStudentByNICAsync(string nic)
        {
            return await _context.Students
                .Include(s => s.User)  // Assuming the NIC is stored in the User entity
                .FirstOrDefaultAsync(s => s.User.NICNumber == nic);  // Modify according to your actual NIC field
        }
        public async Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student) // Include Student details
                .Include(sc => sc.Course)  // Include Course details
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }


    }
}
