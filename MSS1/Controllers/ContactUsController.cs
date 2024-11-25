using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.Interfaces;

namespace MSS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            {
                _contactUsService = contactUsService;
            }

        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact(ContactUsRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _contactUsService.SubmitContactAsync(request);
            return Ok(response);
        }
    }
}