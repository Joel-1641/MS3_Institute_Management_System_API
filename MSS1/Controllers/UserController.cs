using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs;
using MSS1.Interfaces;
using MSS1.Services;

namespace MSS1.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService) // Fixed the parameter name
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Corrected the async call
                var user = await _userService.RegisterUser(userDto);
                return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
