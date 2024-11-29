using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;
using MSS1.Services;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public AdminController(IUserService userService,ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

    


        [HttpPost("students")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentRequestDTO request)
        {
            try
            {
                // Ensure the role is 2 for Student
                if (request.RoleId != 2)
                {
                    return BadRequest(new { Message = "Invalid RoleId. RoleId for student must be 2." });
                }

                var response = await _userService.AddUserAsync(request);
                return CreatedAtAction(nameof(AddStudent), new { userId = response.UserId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("lecturers")]
        public async Task<IActionResult> AddLecturer([FromBody] AddLecturerRequestDTO request)
        {
            try
            {
                // Ensure the role is 3 for Lecturer
                if (request.RoleId != 3)
                {
                    return BadRequest(new { Message = "Invalid RoleId. RoleId for lecturer must be 3." });
                }

                var response = await _userService.AddUserAsync(request);
                return CreatedAtAction(nameof(AddLecturer), new { userId = response.UserId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPost("courses")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseRequestDTO request)
        {
            try
            {
                var response = await _courseService.AddCourseAsync(request);
                return CreatedAtAction(nameof(AddCourse), new { courseId = response.CourseId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the course." });
            }
        }


        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("courses/{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
                return NotFound(new { Message = "Course not found." });

            return Ok(course);
        }

        [HttpPut("courses/{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseRequestDTO request)
        {
            try
            {
                if (courseId != request.CourseId)
                {
                    return BadRequest(new { Message = "CourseId in the URL does not match the one in the request body." });
                }

                var response = await _courseService.UpdateCourseAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the course." });
            }
        }



        [HttpDelete("courses/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var isDeleted = await _courseService.DeleteCourseAsync(courseId);
            if (!isDeleted)
                return NotFound(new { Message = "Course not found." });

            return NoContent();
        }

        [HttpGet("courses/bynamelevel")]
        public async Task<IActionResult> GetCourseByNameAndLevel([FromQuery] string courseName, [FromQuery] string level)
        {
            try
            {
                var response = await _courseService.GetCourseByNameAndLevelAsync(courseName, level);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the course." });
            }
        }
        [HttpGet("lecturers")]
        public async Task<IActionResult> GetAllLecturers()
        {
            var lecturers = await _userService.GetAllLecturersAsync();
            return Ok(lecturers);
        }

        [HttpGet("lecturers/{id}")]
        public async Task<IActionResult> GetLecturerById(int id)
        {
            var lecturer = await _userService.GetLecturerByIdAsync(id);
            if (lecturer == null)
                return NotFound(new { Message = "Lecturer not found." });

            return Ok(lecturer);
        }
        [HttpDelete("lecturers/{id}")]
        public async Task<IActionResult> DeleteLecturerById(int id)
        {
            try
            {
                await _userService.DeleteLecturerAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }


        [HttpPut("lecturers/{id}")]
        public async Task<IActionResult> UpdateLecturer(int id, [FromBody] UpdateLecturerRequestDTO request)
        {
            try
            {
                await _userService.UpdateLecturerAsync(id, request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }



    }
}
