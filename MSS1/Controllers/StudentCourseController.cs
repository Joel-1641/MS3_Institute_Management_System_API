using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;
using MSS1.Services;

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
                // Call the service to record the payment and send notifications
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

        [HttpGet("payment-status/{nic}",Name ="GetPaymentStatus")]
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
        [HttpGet("admin/notifications")]
        public async Task<IActionResult> GetAdminNotifications()
        {
            try
            {
                var notifications = await _studentCourseService.GetAdminNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("payment-notifications/{nic}")]
        public async Task<IActionResult> GetPaymentNotifications(string nic)
        {
            try
            {
                // Retrieve notifications for the student by NIC
                var notifications = await _studentCourseService.GetNotificationsForStudentAsync(nic);

                // Return the notifications to the client
                return Ok(notifications);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // If no student found with the given NIC
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Handle any other exceptions
            }
        }




        // Optionally, you can create a method to mark notifications as read if required:
        //[HttpPost("mark-as-read/{notificationId}")]
        //public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        //{
        //    try
        //    {
        //        // You could add the method in service to mark the notification as read
        //        await _studentCourseService.MarkNotificationAsReadAsync(notificationId);
        //        return Ok(new { message = "Notification marked as read" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
        [HttpGet("report/{nic}")]
        public async Task<IActionResult> GetStudentReportByNICAsync(string nic)
        {
            try
            {
                var report = await _studentCourseService.GetStudentReportByNICAsync(nic);
                return Ok(report); // Return the student report as a response with a 200 OK status
            }
            catch (KeyNotFoundException ex)
            {
                // If student not found, return a 404 Not Found response with the error message
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response in case of other unexpected errors
                return StatusCode(500, new { message = "An error occurred while generating the report.", details = ex.Message });
            }
        }
        [HttpGet("GetCoursesWithStudentCount")]
        public async Task<IActionResult> GetAllCoursesWithStudentCountAsync()
        {
            var courses = await _studentCourseService.GetAllCoursesWithStudentCountAsync();
            return Ok(courses);
        }






    }
}
