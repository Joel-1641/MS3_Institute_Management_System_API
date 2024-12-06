using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.Database;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ITDbContext _context;

        public RoleController(ITDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            // Fetch roles from the database
            var roles = _context.Roles
                                .Select(r => new
                                {
                                    RoleId = r.RoleId,
                                    RoleName = r.RoleName
                                })
                                .ToList();

            return Ok(roles);
        }
    }
}
