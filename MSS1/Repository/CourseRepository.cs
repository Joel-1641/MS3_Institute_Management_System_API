using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
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
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            if (await IsDuplicateCourseAsync(course.CourseName, course.Level))
            {
                throw new ArgumentException($"A course with the name '{course.CourseName}' and level '{course.Level}' already exists.");
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }


        public async Task<Course> UpdateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.CourseId);

            if (existingCourse == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            // Check for duplicates (excluding the current course)
            if (await IsDuplicateCourseAsync(course.CourseName, course.Level, course.CourseId))
            {
                throw new ArgumentException($"A course with the name '{course.CourseName}' and level '{course.Level}' already exists.");
            }

            // Update course fields
            existingCourse.CourseName = course.CourseName;
            existingCourse.Level = course.Level;
            existingCourse.CourseFee = course.CourseFee;
            existingCourse.Description = course.Description;
            existingCourse.CourseDuration = course.CourseDuration;
            existingCourse.CourseImg = course.CourseImg;
            existingCourse.CourseStartDate = course.CourseStartDate;
            existingCourse.CourseEndDate = course.CourseEndDate;

            await _context.SaveChangesAsync();
            return existingCourse;
        }


        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateCourseAsync(string courseName, string level, int? excludeCourseId = null)
        {
            return await _context.Courses.AnyAsync(c =>
                c.CourseName.ToLower() == courseName.ToLower() &&
                c.Level.ToLower() == level.ToLower() &&
                (!excludeCourseId.HasValue || c.CourseId != excludeCourseId.Value));
        }
        public async Task<Course> GetCourseByNameAndLevelAsync(string courseName, string level)
        {
            return await _context.Courses
                                 .FirstOrDefaultAsync(c => c.CourseName.ToLower() == courseName.ToLower() && c.Level.ToLower() == level.ToLower());
        }




    }
}

