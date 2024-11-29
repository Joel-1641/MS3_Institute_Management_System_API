using Microsoft.AspNetCore.Mvc;
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
            var course = new Course
            {
                CourseId = request.CourseId,
                CourseName = request.CourseName,
                Level = request.Level,
                CourseFee = request.CourseFee,
                Description = request.Description
            };

            var updatedCourse = await _courseRepository.UpdateCourseAsync(course);

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
        public async Task<CourseResponseDTO> GetCourseByNameAndLevelAsync(string courseName, string level)
        {
            var course = await _courseRepository.GetCourseByNameAndLevelAsync(courseName, level);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            return new CourseResponseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Level = course.Level,
                CourseFee = course.CourseFee,
                Description = course.Description
            };
        }
       

    }
}
       

          
   