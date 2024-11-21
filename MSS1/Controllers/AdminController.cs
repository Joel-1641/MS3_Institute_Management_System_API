using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users (Admin access required)
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Add a new user (Admin access required)
        /// </summary>
        /// <param name="request">User details</param>
        /// <returns>Created user details</returns>
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDTO request)
        {
            try
            {
                var userResponse = await _userService.AddUserAsync(request);
                return CreatedAtAction(nameof(AddUser), new { userId = userResponse.UserId }, userResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] AddUserRequestDTO request)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, request);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                // Admin validation logic should be implemented here, for now we assume user is an admin

                var deletedUser = await _userService.DeleteUserAsync(userId);
                return Ok(new { Message = "User deleted successfully.", Data = deletedUser });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
