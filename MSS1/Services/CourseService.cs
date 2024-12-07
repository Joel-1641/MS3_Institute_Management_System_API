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
                Description = c.Description,
                CourseImg = c.CourseImg,
                CourseStartDate = c.CourseStartDate,
                CourseEndDate = c.CourseEndDate,
                CourseDuration = c.CourseDuration
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
                Description = course.Description,
                CourseImg = course.CourseImg,
                CourseDuration = course.CourseDuration,
                CourseStartDate = course.CourseStartDate,
                CourseEndDate = course.CourseEndDate
            };
        }

        public async Task<CourseResponseDTO> AddCourseAsync(AddCourseRequestDTO request)
        {
            var course = new Course
            {
                CourseName = request.CourseName,
                Level = request.Level,
                CourseFee = request.CourseFee,
                Description = request.Description,
                CourseImg = request.CourseImg,
                CourseDuration = request.CourseDuration,
                CourseStartDate = request.CourseStartDate,
                CourseEndDate = request.CourseEndDate
            };

            var addedCourse = await _courseRepository.AddCourseAsync(course);

            return new CourseResponseDTO
            {
                CourseId = addedCourse.CourseId,
                CourseName = addedCourse.CourseName,
                Level = addedCourse.Level,
                CourseFee = addedCourse.CourseFee,
                Description = addedCourse.Description,
                CourseImg = addedCourse.CourseImg,
                CourseDuration = addedCourse.CourseDuration,
                CourseStartDate = addedCourse.CourseStartDate,
                CourseEndDate = addedCourse.CourseEndDate
            };
        }


        public async Task<CourseResponseDTO> UpdateCourseAsync(int courseId, UpdateCourseRequestDTO request)
        {
            var course = new Course
            {
                CourseId = courseId, // Use the CourseId from the URL
                CourseName = request.CourseName,
                Level = request.Level,
                CourseFee = request.CourseFee,
                Description = request.Description,
                CourseDuration = request.CourseDuration,
                CourseImg = request.CourseImg,
                CourseStartDate = request.CourseStartDate,
                CourseEndDate = request.CourseEndDate
            };

            var updatedCourse = await _courseRepository.UpdateCourseAsync(course);

            return new CourseResponseDTO
            {
                CourseId = updatedCourse.CourseId,
                CourseName = updatedCourse.CourseName,
                Level = updatedCourse.Level,
                CourseFee = updatedCourse.CourseFee,
                Description = updatedCourse.Description,
                CourseDuration = updatedCourse.CourseDuration,
                CourseImg = updatedCourse.CourseImg,
                CourseStartDate = updatedCourse.CourseStartDate,
                CourseEndDate = updatedCourse.CourseEndDate
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
                Description = course.Description,
                CourseDuration = course.CourseDuration, 
                CourseImg = course.CourseImg,
                CourseEndDate = course.CourseEndDate,
            };
        }

        public async Task<CourseResponseDTO> GetCourseByNameAsync(string courseName)
        {
            var course = await _courseRepository.GetCourseByNameAsync(courseName);

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
                Description = course.Description,
                CourseDuration = course.CourseDuration,
                CourseImg = course.CourseImg,
                CourseEndDate = course.CourseEndDate,
            };
        }
        public async Task<IEnumerable<CourseNameDTO>> GetAllCourseNamesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Select(c => new CourseNameDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Level = c.Level
            }).ToList();
        }


    }
}
       

          
   