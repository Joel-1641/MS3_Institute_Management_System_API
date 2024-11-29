using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface IUserService
    {
        Task<List<LecturerResponseDTO>> GetAllLecturersAsync();
        Task<LecturerResponseDTO> GetLecturerByIdAsync(int lecturerId);
        Task DeleteLecturerAsync(int lecturerId);
        Task UpdateLecturerAsync(int lecturerId, UpdateLecturerRequestDTO request);
        Task<AddUserResponseDTO> AddUserAsync(AddUserRequestDTO request);
    }
}
