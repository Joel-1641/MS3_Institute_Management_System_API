using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.Interfaces;


namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        /// <summary>
        /// Get all lecturers.
        /// </summary>
        /// <returns>List of lecturers</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllLecturers()
        {
            try
            {
                var lecturers = await _lecturerService.GetAllLecturersAsync();
                return Ok(lecturers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        /// <summary>
        /// Get lecturer details by ID.
        /// </summary>
        /// <param name="lecturerId">Lecturer ID</param>
        /// <returns>Lecturer details</returns>
        //[HttpGet("{lecturerId}")]
        //public async Task<IActionResult> GetLecturerById(int lecturerId)
        //{
        //    try
        //    {
        //        var lecturer = await _lecturerService.GetLecturerByIdAsync(lecturerId);
        //        return Ok(lecturer);
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

        /// <summary>
        /// Get courses assigned to a lecturer.
        /// </summary>
        /// <param name="lecturerId">Lecturer ID</param>
        /// <returns>List of courses taught by the lecturer</returns>
        //[HttpGet("{lecturerId}/courses")]
        //public async Task<IActionResult> GetCoursesByLecturer(int lecturerId)
        //{
        //    try
        //    {
        //        var courses = await _lecturerService.GetCoursesByLecturerAsync(lecturerId);
        //        return Ok(courses);
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

        /// <summary>
        /// Assign courses to a lecturer.
        /// </summary>
        /// <param name="lecturerId">Lecturer ID</param>
        /// <param name="courses">List of course names</param>
        /// <returns>Response indicating success or failure</returns>
        //[HttpPost("{lecturerId}/courses")]
        //public async Task<IActionResult> AssignCoursesToLecturer(int lecturerId, [FromBody] List<string> courses)
        //{
        //    try
        //    {
        //        await _lecturerService.AssignCoursesToLecturerAsync(lecturerId, courses);
        //        return Ok(new { Message = "Courses assigned successfully." });
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

        /// <summary>
        /// Delete a lecturer by ID.
        /// </summary>
        /// <param name="lecturerId">Lecturer ID</param>
        /// <returns>Response indicating success or failure</returns>
        //[HttpDelete("{lecturerId}")]
        //public async Task<IActionResult> DeleteLecturer(int lecturerId)
        //{
        //    try
        //    {
        //        await _lecturerService.DeleteLecturerAsync(lecturerId);
        //        return Ok(new { Message = "Lecturer deleted successfully." });
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
