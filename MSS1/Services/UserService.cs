using MSS1.DTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> RegisterUser (UserDto userDto)
        {
            var role = await _userRepository.GetRoleById(userDto.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            var user = new User
            {
                FullName = userDto.FullName,
                RoleId = role.RoleId,
                Authentication = new Authentication
                {
                    Email = userDto.Email
                }
            };

            return await _userRepository.RegisterUser(user, userDto.Password);
        }

    }
}
