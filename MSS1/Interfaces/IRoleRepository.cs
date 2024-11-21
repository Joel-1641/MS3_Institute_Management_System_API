using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByIdAsync(int roleId); // Fetch role by its ID
        Task<List<Role>> GetAllRolesAsync();
    }
}
