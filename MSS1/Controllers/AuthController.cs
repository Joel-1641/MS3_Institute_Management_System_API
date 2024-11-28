using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Use primary constructor to declare dependencies
        private readonly IAuthenticServices _authService;
        private readonly IConfiguration _configuration;

        // Primary constructor to initialize the dependencies
        public AuthController(IAuthenticServices authService, IConfiguration configuration) =>
            (_authService, _configuration) = (authService, configuration);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="requestDTO">Registration details.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO requestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Validate email domain
                await _authService.ValidateEmail(requestDTO.Email);

                // Proceed with registration
                var responseDTO = await _authService.RegisterUserAsync(requestDTO);
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }



        /// <summary>
        /// Authenticates a user and provides a JWT token.
        /// </summary>
        /// <param name="requestDTO">Login details.</param>
        /// <returns>A JWT token and user details.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Error = "Invalid login details." });

            try
            {
                var secretKey = _configuration["Jwt:SecretKey"];
                if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                    return StatusCode(500, new { Error = "Invalid JWT secret key configuration." });

                var responseDTO = await _authService.LoginAsync(requestDTO, secretKey);
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Logs out a user by invalidating their token.
        /// </summary>
        /// <param name="token">The JWT token to invalidate.</param>
        /// <returns>A success message.</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenRequestDTO tokenDTO)
        {
            if (string.IsNullOrWhiteSpace(tokenDTO.Token))
                return BadRequest(new { Error = "Token cannot be empty." });

            try
            {
                await _authService.LogoutAsync(tokenDTO.Token);
                return Ok(new { Message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    

            // Login for Student
            [HttpPost("login/student")]
            public async Task<IActionResult> LoginStudent([FromBody] LoginRequestDTO requestDTO)
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = "Invalid login details." });

                try
                {
                    var secretKey = _configuration["Jwt:SecretKey"];
                    if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                        return StatusCode(500, new { Error = "Invalid JWT secret key configuration." });

                    var responseDTO = await _authService.LoginAsync(requestDTO, secretKey);
                    return Ok(responseDTO);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }

            // Login for Lecturer
            [HttpPost("login/lecturer")]
            public async Task<IActionResult> LoginLecturer([FromBody] LoginRequestDTO requestDTO)
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Error = "Invalid login details." });

                try
                {
                    var secretKey = _configuration["Jwt:SecretKey"];
                    if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                        return StatusCode(500, new { Error = "Invalid JWT secret key configuration." });

                    var responseDTO = await _authService.LoginAsync(requestDTO, secretKey);
                    return Ok(responseDTO);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }

            // Logout for Student
            [HttpPost("logout/student")]
            public async Task<IActionResult> LogoutStudent([FromBody] TokenRequestDTO tokenDTO)
            {
                if (string.IsNullOrWhiteSpace(tokenDTO.Token))
                    return BadRequest(new { Error = "Token cannot be empty." });

                try
                {
                    await _authService.LogoutAsync(tokenDTO.Token);
                    return Ok(new { Message = "Logged out successfully." });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }

            // Logout for Lecturer
            [HttpPost("logout/lecturer")]
            public async Task<IActionResult> LogoutLecturer([FromBody] TokenRequestDTO tokenDTO)
            {
                if (string.IsNullOrWhiteSpace(tokenDTO.Token))
                    return BadRequest(new { Error = "Token cannot be empty." });

                try
                {
                    await _authService.LogoutAsync(tokenDTO.Token);
                    return Ok(new { Message = "Logged out successfully." });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }
        }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO requestDTO)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { Error = "Invalid request data." });

        //    try
        //    {
        //        var response = await _authService.ForgotPasswordAsync(requestDTO);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        //{
        //    try
        //    {
        //        await _authService.ResetPasswordAsync(request);
        //        return Ok(new { message = "Password reset successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
    }

