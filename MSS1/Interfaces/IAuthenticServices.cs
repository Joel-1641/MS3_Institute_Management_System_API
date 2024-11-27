using Microsoft.EntityFrameworkCore;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IAuthenticServices
    {
        Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO requestDTO);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO, string secretKey);
        Task LogoutAsync(string token);
        Task AddAdminAsync(Admin admin);
        Task ValidateEmailDomain(string email);
        Task ValidateEmail(string email);


        // Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO requestDTO);
        // Task ResetPasswordAsync(ResetPasswordRequestDTO request);
    }
}
