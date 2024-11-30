using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // API to get all course names
        [HttpGet("GetCourseNames")]
        public async Task<IActionResult> GetCourseNamesAsync()
        {
            var courses = await _courseService.GetAllCourseNamesAsync();
            return Ok(courses);
        }
    }

}
