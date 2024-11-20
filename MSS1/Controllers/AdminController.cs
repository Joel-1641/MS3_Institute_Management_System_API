using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            /// Get all users (Admins and Students).
            /// Only accessible by Admins.
            /// </summary>
            [HttpGet("users")]
            public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
        }
}
