using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentCourseController : ControllerBase
    {
        private readonly IStudentCourseService _studentCourseService;

        public StudentCourseController(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        [HttpPost("add-courses")]
        public async Task<IActionResult> AddStudentCourses([FromBody] AddStudentCourseRequestDTO request)
        {
            try
            {
                await _studentCourseService.AddStudentCoursesAsync(request);
                return Ok("Courses added successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            var courses = await _studentCourseService.GetCoursesByStudentIdAsync(studentId);
            return Ok(courses);
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudentCourses()
        {
            var studentCourses = await _studentCourseService.GetAllStudentCoursesAsync();
            return Ok(studentCourses);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentWithCoursesById(int studentId)
        {
            var studentCourses = await _studentCourseService.GetStudentWithCoursesByIdAsync(studentId);
            return Ok(studentCourses);
        }

        [HttpGet("course/{courseId}/students-count")]
        public async Task<IActionResult> GetStudentCountForCourse(int courseId)
        {
            var count = await _studentCourseService.GetStudentCountForCourseAsync(courseId);
            return Ok(new { CourseId = courseId, StudentCount = count });
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _studentCourseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("student/{studentId}/nic")]
        public async Task<IActionResult> GetStudentNIC(int studentId)
        {
            try
            {
                var nic = await _studentCourseService.GetStudentNICByIdAsync(studentId);
                return Ok(new { StudentId = studentId, NIC = nic });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
