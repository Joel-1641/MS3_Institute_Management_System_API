using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
