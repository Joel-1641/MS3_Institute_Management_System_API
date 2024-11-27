using Microsoft.EntityFrameworkCore;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSS1.Services
{
    public class AuthenticService(
        IAuthenticationRepository repository,
        IPasswordHasher passwordHasher,
        ITokenRepository tokenRepository) : IAuthenticServices
    {
        private readonly IAuthenticationRepository _repository = repository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenRepository _tokenRepository = tokenRepository;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// 
        public async Task ValidateEmailDomain(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required.");

            var emailParts = email.Split('@');
            if (emailParts.Length != 2 || emailParts[1].ToLower() != "gmail.com")
                throw new Exception("Only Gmail addresses are allowed.");
        }
        public async Task ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required.");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
                throw new Exception("Invalid email format.");

            var domain = email.Split('@').LastOrDefault();
            if (domain == null || domain.ToLower() != "gmail.com")
                throw new Exception("Only Gmail addresses are allowed.");
        }


        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO requestDTO)
        {
            if (requestDTO == null) throw new ArgumentNullException(nameof(requestDTO));

            if (requestDTO.RoleId != 1)
                throw new Exception("Only Admins are allowed to register.");

            var existingUser = await _repository.GetUserByEmailAsync(requestDTO.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            var role = await _repository.GetRoleByIdAsync(requestDTO.RoleId)
                ?? throw new Exception("Invalid role ID.");

            var (hashedPassword, salt) = _passwordHasher.HashPassword(requestDTO.Password);

            var user = new User
            {
                FullName = requestDTO.FullName,
                RoleId = requestDTO.RoleId,
                Authentication = new Authentication
                {
                    Email = requestDTO.Email,
                    HashedPassword = hashedPassword,
                    PasswordSalt = salt
                }
            };

            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();

            // Add Admin to Admins table
            var admin = new Admin
            {
                UserId = user.UserId,
                fullName = user.FullName,
                Email = user.Authentication.Email,
               // RoleId = user.RoleId
            };

            await _repository.AddAdminAsync(admin);
            await _repository.SaveChangesAsync();

            return new RegisterUserResponseDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Authentication.Email,
                RoleName = role.RoleName,
                Message = "Admin registered successfully."
            };
        }



        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO, string secretKey)
        {
            var auth = await _repository.GetAuthenticationByEmailAsync(requestDTO.Email)
                ?? throw new Exception("Invalid email or password.");

            if (!_passwordHasher.VerifyPassword(requestDTO.Password, auth.HashedPassword, auth.PasswordSalt))
                throw new Exception("Invalid email or password.");

            var token = TokenHelper.GenerateJwtToken(auth.User, secretKey);

            return new LoginResponseDTO
            {
                Token = token,
                FullName = auth.User.FullName,
                Role = auth.User.Role.RoleName
            };
        }

        /// <summary>
        /// Logs out a user by invalidating their token.
        /// </summary>
        public async Task LogoutAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");

            await _tokenRepository.InvalidateTokenAsync(token);
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        //public async Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO requestDTO)
        //{
        //    var auth = await _repository.GetAuthenticationByEmailAsync(requestDTO.Email)
        //        ?? throw new Exception("User with this email does not exist.");

        //    var resetToken = Guid.NewGuid().ToString();
        //    auth.PasswordResetToken = resetToken;
        //    auth.TokenExpiration = DateTime.UtcNow.AddHours(1);
        //    await _repository.SaveChangesAsync();

        //    SendEmail(auth.Email, "Password Reset Link",
        //        $"Click the link to reset your password: https://example.com/reset-password?token={resetToken}");

        //    return new ForgotPasswordResponseDTO
        //    {
        //        Message = "Password reset link has been sent to the provided email address."
        //    };
        //}

        /// <summary>
        /// Resets a user's password using a reset token.
        /// </summary>
        //public async Task ResetPasswordAsync(ResetPasswordRequestDTO request)
        //{
        //    var auth = await _repository.GetByResetTokenAsync(request.Token)
        //        ?? throw new Exception("Invalid or expired token.");

        //    if (auth.TokenExpiration < DateTime.UtcNow)
        //        throw new Exception("Expired token.");

        //    var (hashedPassword, salt) = _passwordHasher.HashPassword(request.NewPassword);
        //    await _repository.UpdatePasswordAsync(auth, hashedPassword, salt);
        //}

        /// <summary>
        /// Sends an email. Replace with a real email service in production.
        /// </summary>
        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage("noreply@example.com", toEmail)
                {
                    Subject = subject,
                    Body = body
                };

                using var smtpClient = new SmtpClient("smtp.example.com");
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }
        public async Task AddAdminAsync(Admin admin)
        {
            if (admin == null) throw new ArgumentNullException(nameof(admin));
            await _repository.AddAdminAsync(admin);
        }
       

       




    }
}
