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
        private readonly IStudentService _studentService;
        private readonly ILecturerService _lecturerService;


        public AdminController(IUserService userService,ICourseService courseService, IStudentService studentService,ILecturerService lecturerService)
        {
            _userService = userService;
            _courseService = courseService;
            _studentService = studentService;
            _lecturerService = lecturerService;
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
        public async Task<IActionResult> UpdateCourseAsync(int courseId, [FromBody] UpdateCourseRequestDTO request)
        {
            try
            {
                // Pass the CourseId explicitly to the service
                var response = await _courseService.UpdateCourseAsync(courseId, request);
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

        [HttpGet("courses/byname")]
        public async Task<IActionResult> GetCourseByName([FromQuery] string courseName)
        {
            try
            {
                var response = await _courseService.GetCourseByNameAsync(courseName);
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

       
        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }


        // Get Student By Id
        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpGet("by-nic/{nicNumber}")]
        public async Task<IActionResult> GetStudentByNICNumber(string nicNumber)
        {
            try
            {
                var student = await _studentService.GetStudentByNICNumberAsync(nicNumber);
                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        // Delete Student By Id
        [HttpDelete("students/{id}")]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Update Student By Id
        [HttpPut("students/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequestDTO request)
        {
            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(id, request);
                return Ok(updatedStudent);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        // Login API for both Student and Lecturer
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null)
                return BadRequest("Invalid login request.");

            try
            {
                var loginResponse = await _userService.LoginAsync(loginRequest);
                return Ok(loginResponse); // Return the token and user details
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(new { message = ex.Message }); // Invalid login credentials
            }
        }

        // Logout API
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Client-side should handle token invalidation by removing it from storage
            _userService.LogoutAsync();
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
