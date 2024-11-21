using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task<AddUserResponseDTO> AddUserAsync(AddUserRequestDTO request);
        Task<AddUserResponseDTO> UpdateUserAsync(int userId, AddUserRequestDTO request);
    }
}
