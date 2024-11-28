using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface ILecturerService
    {
        Task<IEnumerable<LecturerResponseDTO>> GetAllLecturersAsync();
       // Task<LecturerResponseDTO> GetLecturerByIdAsync(int lecturerId);
       // Task<List<string>> GetCoursesByLecturerAsync(int lecturerId);
      //  Task AssignCoursesToLecturerAsync(int lecturerId, List<string> courses);
      //  Task DeleteLecturerAsync(int lecturerId);
    }


}
