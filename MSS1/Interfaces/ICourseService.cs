using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
        Task<CourseResponseDTO> GetCourseByIdAsync(int courseId);
        Task<CourseResponseDTO> AddCourseAsync(AddCourseRequestDTO request);
        Task<CourseResponseDTO> UpdateCourseAsync(UpdateCourseRequestDTO request);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<CourseResponseDTO> GetCourseByNameAndLevelAsync(string courseName, string level);
        Task<IEnumerable<CourseNameDTO>> GetAllCourseNamesAsync();
        Task<CourseResponseDTO> GetCourseByNameAsync(string courseName);
        //Task<IActionResult> GetCourseByNameAndLevel([FromQuery] string courseName, [FromQuery] string level);
    }
}
