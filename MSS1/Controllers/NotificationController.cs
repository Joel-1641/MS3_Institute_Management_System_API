using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
        {
            try
            {
                // Pass the actual request object to the SendNotificationAsync method
                await _notificationService.SendNotificationAsync(request);
                return Ok(new { Message = "Notification sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
