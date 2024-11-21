using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class CourseRepository: ICourseRepository
    {
        private readonly ITDbContext _context;

        public CourseRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
        }
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }
        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await GetCourseByIdAsync(courseId);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

