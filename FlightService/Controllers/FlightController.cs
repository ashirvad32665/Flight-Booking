using System.Diagnostics;
using FlightService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers
{
    public class FlightController : ControllerBase
    {
        private readonly IFlight repository;
        public FlightController(IFlight repo)
        {
            repository= repo;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddFlight(Flight flight)
        {
            try
            {
                var res = await repository.AddFlight(flight);
                return Ok("Added data successfully");
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
        [HttpGet("getFlight")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightByDepartureDate([FromQuery] string from, [FromQuery] string to, [FromQuery] DateOnly date)
        {
            try
            {
                var res = await repository.GetFlightByDepartureDate(from, to, date);
                return Ok(res);
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
        [HttpDelete("del/{flightId}")]
        public async Task<IActionResult> RemoveResult(int flightId)
        {
            try
            {
                var res = await repository.RemoveFlight(flightId);
                return Ok("Deleted succesfully");
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
        [HttpPut("update/{flightId}")]
        public async Task<ActionResult<Flight>> UpdateAvailableSeat(int flightId, int AvailableSeat)
        {
            try
            {
                var res = await repository.UpdateAvailableSeat(flightId, AvailableSeat);
                return Ok(res);
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
        [HttpPut("updateFlightDetail/{flightId}")]
        public async Task<ActionResult<Flight>> UpdateFlightDetails(int flightId, Flight flight)
        {
            try
            {
                var res = await repository.UpdateFlight(flightId, flight);
                return Ok(res);
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

        [HttpGet("{flightId}")]
        public async Task<ActionResult<Flight>> GetFlightById(int flightId)
        {
            try
            {
                var res = await repository.GetFlightById(flightId);
                return Ok(res);
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
    }
}
