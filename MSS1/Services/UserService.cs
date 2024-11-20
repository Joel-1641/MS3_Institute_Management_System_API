using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            // Map users to DTOs
            var userResponseDTOs = new List<UserResponseDTO>();
            foreach (var user in users)
            {
                var userDTO = new UserResponseDTO
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    RoleName = user.Role?.RoleName,
                    //AdminRoleType = user.Admin?.AdminRoleType, // Only for Admins
                    ProfilePicture = user.Student?.ProfilePicture // Only for Students
                };
                userResponseDTOs.Add(userDTO);
            }

            return userResponseDTOs;
        }
    }
}
