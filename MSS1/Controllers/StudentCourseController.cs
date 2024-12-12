using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
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
                return BadRequest(ex.Message); // Return detailed error messages for duplicates
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

        //[HttpGet("student/{studentId}")]
        //public async Task<IActionResult> GetStudentWithCoursesById(int studentId)
        //{
        //    var studentCourses = await _studentCourseService.GetStudentWithCoursesByIdAsync(studentId);
        //    return Ok(studentCourses);
        //}

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
        [HttpGet("student/nic/{nic}/courses")]
        public async Task<IActionResult> GetCoursesByNIC(string nic)
        {
            try
            {
                var courses = await _studentCourseService.GetCoursesByNICAsync(nic);
                return Ok(courses);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("total-fee/{nic}")]
        public async Task<ActionResult<StudentTotalFeeResponseDTO>> GetTotalFeeByNICAsync(string nic)
        {
            try
            {
                var result = await _studentCourseService.GetTotalFeeByNICAsync(nic);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("record-payment")]
        public async Task<IActionResult> RecordPayment([FromBody] RecordPaymentRequestDTO request)
        {
            try
            {
                await _studentCourseService.RecordPaymentAsync(request.NIC, request.Amount);
                return Ok("Payment recorded successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("payment-status/{nic}")]
        public async Task<IActionResult> GetPaymentStatus(string nic)
        {
            try
            {
                var result = await _studentCourseService.GetPaymentStatusByNICAsync(nic);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpGet("payment-status")]
        public async Task<IActionResult> GetAllStudentsPaymentStatus()
        {
            try
            {
                var result = await _studentCourseService.GetAllStudentsPaymentStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpGet("cumulative-payment-status")]
        public async Task<IActionResult> GetCumulativePaymentStatus()
        {
            try
            {
                var result = await _studentCourseService.GetCumulativePaymentStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }
        [HttpGet("totalcoursesbynic/{nic}")]
        public async Task<IActionResult> GetTotalCoursesByNIC(string nic)
        {
            try
            {
                // Get the total number of courses by NIC
                var totalCourses = await _studentCourseService.GetTotalCoursesByNICAsync(nic);

                // Return the result as JSON
                return Ok(new { TotalCourses = totalCourses });
            }
            catch (KeyNotFoundException ex)
            {
                // If student is not found
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }





    }
}
