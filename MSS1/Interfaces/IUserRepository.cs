using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user, string password);
        Task<Role> GetRoleById(int roleId);
    }
}
