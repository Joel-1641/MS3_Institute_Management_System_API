using MSS1.DTOs;
using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(UserDto userDto);
    }
}
