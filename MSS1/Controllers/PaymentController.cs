using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // 1. Process Payment
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentRequest)
        {
            try
            {
                await _paymentService.ProcessPaymentAsync(paymentRequest);
                return Ok(new { Message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 2. Get Total Dues for a Course
        [HttpGet("course/{courseId}/dues")]
        public async Task<IActionResult> GetTotalDues(int courseId)
        {
            try
            {
                var totalDue = await _paymentService.GetTotalDueAmountAsync(courseId);
                return Ok(new { CourseId = courseId, TotalDueAmount = totalDue });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 3. Notify Overdue Payments
        [HttpPost("notify-overdue")]
        public async Task<IActionResult> NotifyOverduePayments()
        {
            try
            {
                await _paymentService.NotifyOverduePaymentsAsync();
                return Ok(new { Message = "Overdue notifications sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
