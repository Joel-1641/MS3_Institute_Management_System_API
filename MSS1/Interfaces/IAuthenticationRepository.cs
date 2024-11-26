using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
        Task<Authentication> GetAuthenticationByEmailAsync(string email);
        Task<Authentication> GetByResetTokenAsync(string token);
        //Task UpdatePasswordAsync(Authentication auth, string newHashedPassword, string newSalt);

    }
}
