using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class CourseService: ICourseService
    {

        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseResponseDTO> AddCourseAsync(AddCourseRequestDTO request)
        {
            // Create new course entity
            var course = new Course
            {
                CourseName = request.CourseName,
                Level = request.Level,
                CourseFee = request.CourseFee,
                Description = request.Description
            };

            // Add the course using the repository
            var addedCourse = await _courseRepository.AddCourseAsync(course);

            // Return the added course as a response DTO
            return new CourseResponseDTO
            {
                CourseId = addedCourse.CourseId,
                CourseName = addedCourse.CourseName,
                Level = addedCourse.Level,
                CourseFee = addedCourse.CourseFee,
                Description = addedCourse.Description
            };
        }
        public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();

            // Map courses to response DTOs
            return courses.Select(course => new CourseResponseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Level = course.Level,
                CourseFee = course.CourseFee,
                Description = course.Description
            });
        }
        public async Task<CourseResponseDTO> UpdateCourseAsync(int courseId, AddCourseRequestDTO request)
        {
            // Fetch the existing course
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            // Update the course properties
            course.CourseName = request.CourseName;
            course.Level = request.Level;
            course.CourseFee = request.CourseFee;
            course.Description = request.Description;

            // Save the changes
            var updatedCourse = await _courseRepository.UpdateCourseAsync(course);

            // Map the updated course to a response DTO
            return new CourseResponseDTO
            {
                CourseId = updatedCourse.CourseId,
                CourseName = updatedCourse.CourseName,
                Level = updatedCourse.Level,
                CourseFee = updatedCourse.CourseFee,
                Description = updatedCourse.Description
            };
        }
        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var courseExists = await _courseRepository.GetCourseByIdAsync(courseId);
            if (courseExists == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            return await _courseRepository.DeleteCourseAsync(courseId);
        }
    }
}
