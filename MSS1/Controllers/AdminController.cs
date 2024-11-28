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

        /// <summary>
        /// Get all users (Admin access required)
        /// </summary>
        /// <returns>List of users</returns>
        ////[HttpGet("users")]
        ////public async Task<IActionResult> GetAllUsers()
        ////{
        ////    try
        ////    {
        ////        var users = await _userService.GetAllUsersAsync();
        ////        return Ok(users);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, new { Message = ex.Message });
        ////    }
        ////}



        /// <summary>
        /// Add a new user (Admin access required)
        /// </summary>
        /// <param name="request">User details</param>
        /// <returns>Created user details</returns>
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



        //[HttpPut("users/{userId}")]
        //public async Task<IActionResult> UpdateUser(int userId, [FromBody] AddUserRequestDTO request)
        //{
        //    try
        //    {
        //        var updatedUser = await _userService.UpdateUserAsync(userId, request);
        //        return Ok(updatedUser);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}
        //[HttpDelete("users/{userId}")]
        //public async Task<IActionResult> DeleteUser(int userId)
        //{
        //    try
        //    {
        //        // Admin validation logic should be implemented here, for now we assume user is an admin

        //        var deletedUser = await _userService.DeleteUserAsync(userId);
        //        return Ok(new { Message = "User deleted successfully.", Data = deletedUser });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}

        //[HttpPost("courses")]
        //public async Task<IActionResult> AddCourse([FromBody] AddCourseRequestDTO request)
        //{
        //    try
        //    {
        //        // Add the course using the service
        //        var courseResponse = await _courseService.AddCourseAsync(request);
        //        return CreatedAtAction(nameof(AddCourse), new { courseId = courseResponse.CourseId }, courseResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}
        //[HttpGet("courses")]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    try
        //    {
        //        var courses = await _courseService.GetAllCoursesAsync();
        //        return Ok(courses);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}
        //[HttpPut("courses/{courseId}")]
        //public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] AddCourseRequestDTO request)
        //{
        //    try
        //    {
        //        var updatedCourse = await _courseService.UpdateCourseAsync(courseId, request);
        //        return Ok(new { Message = "Course updated successfully.", Data = updatedCourse });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}
        //[HttpDelete("courses/{courseId}")]
        //public async Task<IActionResult> DeleteCourse(int courseId)
        //{
        //    try
        //    {
        //        var result = await _courseService.DeleteCourseAsync(courseId);
        //        if (result)
        //        {
        //            return Ok(new { Message = "Course deleted successfully." });
        //        }
        //        return NotFound(new { Message = "Course not found." });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}
    }
}
