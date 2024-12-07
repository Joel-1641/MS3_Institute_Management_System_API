using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }


        //[HttpGet("profile")]
        //public async Task<IActionResult> GetProfile([FromQuery] int studentId)
        //{
        //    try
        //    {
        //        var profile = await _service.GetStudentProfile(studentId);
        //        return Ok(profile);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred.", details = ex.Message });
        //    }
        //}



        //[HttpPut("profile")]
        //public async Task<IActionResult> UpdateProfile([FromQuery] int studentId, [FromBody] UpdateStudentRequestDTO request)
        //{
        //    await _service.UpdateStudentProfile(studentId, request);
        //    return NoContent();
        //}

        //[HttpGet("courses")]
        //public async Task<IActionResult> GetCourses([FromQuery] int studentId)
        //{
        //    var courses = await _service.GetStudentCourses(studentId);
        //    return Ok(courses);
        //}

        //[HttpPost("courses/{courseId}/enroll")]
        //public async Task<IActionResult> EnrollInCourse([FromQuery] int studentId, [FromRoute] int courseId)
        //{
        //    await _service.EnrollInCourse(studentId, courseId);
        //    return Ok();
        //}

        //[HttpGet("payments")]
        //public async Task<IActionResult> GetPayments([FromQuery] int studentId)
        //{
        //    var payments = await _service.GetStudentPayments(studentId);
        //    return Ok(payments);
        //}

        //[HttpPost("payments")]
        //public async Task<IActionResult> AddPayment([FromBody] AddPaymentRequestDTO request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Request body cannot be null.");
        //    }

        //    try
        //    {
        //        // Call the service to handle the payment addition logic
        //        await _service.AddPayment(request);
        //        return Ok(new { message = "Payment successfully added." });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        // If the Student or Course is not found
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Generic error handling
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", details = ex.Message });
        //    }
        //}
        //[HttpPost("courses/{courseId}/enroll")]
        //public async Task<IActionResult> EnrollInCourse([FromQuery] int studentId, [FromRoute] int courseId)
        //{
        //    try
        //    {
        //        await _service.EnrollInCourse(studentId, courseId);
        //        return Ok(new { message = "Student enrolled in the course successfully." });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred.", details = ex.Message });
        //    }
        //}


    }
}
