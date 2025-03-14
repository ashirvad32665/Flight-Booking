using CheckInService.Process;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        CheckInProcess process;
        public CheckInController(CheckInProcess process)
        {
            this.process = process;
        }
        [HttpPost("{bookingId}")]
        public async Task<IActionResult> CheckIn(int bookingId)
        {
            var (success, message, confirmationNumber) = await process.CheckInAsync(bookingId);

            if (!success)
                return BadRequest(new { Message = message });

            return Ok(new
            {
                Message = message,
                ConfirmationNumber = confirmationNumber
            });
        }
    }
}
