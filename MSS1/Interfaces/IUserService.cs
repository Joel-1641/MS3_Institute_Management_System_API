using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    }
}
