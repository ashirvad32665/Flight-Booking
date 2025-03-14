using BookingService.Models;
using BookingService.Process;
using BookingService.Repository;
using CommonUse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    //[CustomAuthentication(Roles ="User")]
    public class BookingController : ControllerBase
    {
        private readonly BookingProcess process;
        public BookingController(BookingProcess process)
        {
            this.process = process;
        }
        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] BookingRequest request)
        {
            try
            {
                //var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                string referenceNumber = await process.BookFlightAsync(request);
                return Ok(new { Message = "Booking successful", ReferenceNumber = referenceNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{referenceNumber}")]
        public async Task<IActionResult> GetBookingDetails(string referenceNumber)
        {
            try
            {
                var bookingDetails = await process.GetBookingDetailsAsync(referenceNumber);
                return Ok(bookingDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
