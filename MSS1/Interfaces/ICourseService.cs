using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface ICourseService
    {
        Task<CourseResponseDTO> AddCourseAsync(AddCourseRequestDTO request);
        Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
    }
}
