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
        public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Select(c => new CourseResponseDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Level = c.Level,
                CourseFee = c.CourseFee,
                Description = c.Description
            });
        }

        public async Task<CourseResponseDTO> GetCourseByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null) return null;

            return new CourseResponseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Level = course.Level,
                CourseFee = course.CourseFee,
                Description = course.Description
            };
        }

        public async Task<CourseResponseDTO> AddCourseAsync(AddCourseRequestDTO request)
        {
            if (await _courseRepository.IsDuplicateCourseAsync(request.CourseName, request.Level))
            {
                throw new ArgumentException($"A course with the name '{request.CourseName}' and level '{request.Level}' already exists.");
            }


            var course = new Course
            {
                CourseName = request.CourseName,
                Level = request.Level,
                CourseFee = request.CourseFee,
                Description = request.Description
            };

            var addedCourse = await _courseRepository.AddCourseAsync(course);

            return new CourseResponseDTO
            {
                CourseId = addedCourse.CourseId,
                CourseName = addedCourse.CourseName,
                Level = addedCourse.Level,
                CourseFee = addedCourse.CourseFee,
                Description = addedCourse.Description
            };
        }

        public async Task<CourseResponseDTO> UpdateCourseAsync(UpdateCourseRequestDTO request)
        {
            // Find the course by ID
            var course = await _courseRepository.GetCourseByIdAsync(request.CourseId);
            if (course == null)
                throw new ArgumentException("Course not found.");

            // Check if a duplicate exists with the same CourseName and Level (but not the same CourseId)
            if (await _courseRepository.IsDuplicateCourseAsync(request.CourseName, request.Level) &&
                !(course.CourseName == request.CourseName && course.Level == request.Level))
            {
                throw new ArgumentException($"A course with the name '{request.CourseName}' and level '{request.Level}' already exists.");
            }

            // Update course details
            course.CourseName = request.CourseName;
            course.Level = request.Level;
            course.CourseFee = request.CourseFee;
            course.Description = request.Description;

            var updatedCourse = await _courseRepository.UpdateCourseAsync(course);

            // Return the updated course as DTO
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
            return await _courseRepository.DeleteCourseAsync(courseId);
        }
    }
}
       

          
   