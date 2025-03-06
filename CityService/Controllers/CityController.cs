using System.Diagnostics;
using CityService.Process;
using CityService.Repository;
using CommonUse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        //private readonly ICity _cityRepository;
        //CityProcess process;
        //ILogger<CityController> logger;
        ICity _cityRepository;
        public CityController(ICity cityRepository) {
            _cityRepository = cityRepository;
        }
        //public CityController(CityProcess p, ILogger<CityController> log) =>
        //    (process, logger) = (p, log);

        // Add a new city
        [HttpPost("Add")]
        public async Task<ActionResult<bool>> AddCityAsync(City city)
        {
            try
            {
                // Call the repository to add the city
                var result = await _cityRepository.AddCityAsync(city);

                // Return true if the city was added successfully
                if (result)
                {
                    return Ok(true);  // HTTP 200 OK response
                }
                else
                {
                    return BadRequest(new { Message = "Failed to add the city." }); // HTTP 400 Bad Request if adding fails
                }
            }
            catch (InvalidCityDataException ex)
            {
                // Handle invalid city data (e.g., missing or incorrect fields)
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { Message = ex.Message });  // HTTP 400 Bad Request with the error message
            }
            catch (KeyAlreadyExistsException ex)
            {
                // Handle case where the city already exists
                Console.WriteLine($"Error: {ex.Message}");
                return Conflict(new { Message = ex.Message });  // HTTP 409 Conflict if the city already exists
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });  // HTTP 500 Internal Server Error
            }
        }


        // Update city details
        [HttpPut("update/{cityCode}")]
        public async Task<ActionResult<City>> UpdateCityAsync(string cityCode, City city)
        {
            try
            {
                // Call the repository to update the city details
                var updatedCity = await _cityRepository.UpdateCityAsync(cityCode, city);

                // If city is not found, return a 404 response
                if (updatedCity == null)
                {
                    throw new KeyNotExistException($"City with CityCode {cityCode} does not exist.");
                }

                // Return the updated city
                return Ok(updatedCity);  // HTTP 200 OK response with the updated city
            }
            catch (KeyNotExistException ex)
            {
                // Handle the case when the city does not exist
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(new { Message = ex.Message });  // HTTP 404 Not Found response
            }
            catch (InvalidCityDataException ex)
            {
                // Handle invalid city data (e.g., missing or incorrect fields)
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { Message = ex.Message });  // HTTP 400 Bad Request response
            }
            
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });  // HTTP 500 Internal Server Error
            }
        }


        // Delete a city by CityCode
        [HttpDelete("Delete/{cityCode}")]
        public async Task<ActionResult<bool>> DeleteCityAsync(string cityCode)
        {
            try
            {
                // Call the repository to delete the city
                var result = await _cityRepository.DeleteCityAsync(cityCode);

                // If the city does not exist, the repository will return false, so we handle that case
                if (!result)
                {
                    return NotFound(new { Message = $"City with CityCode {cityCode} not found." });  // HTTP 404 Not Found
                }

                // Return success if the city is successfully marked as inactive (soft delete)
                return Ok(new { Message = "City successfully marked as inactive." });  // HTTP 200 OK
            }
            catch (KeyNotExistException ex)
            {
                // Handle the case where the city with the provided cityCode does not exist
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(new { Message = ex.Message });  // HTTP 404 Not Found
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });  // HTTP 500 Internal Server Error
            }
        }


        // Update airport charge for a specific city
        [HttpPut("update-airport-charge/{cityCode}")]
        public async Task<ActionResult<City>> UpdateAirportChargeAsync(string cityCode, int airportCharge)
        {
            try
            {
                // Call the repository to update the airport charge
                var updatedCity = await _cityRepository.UpdateAirportChargeAsync(cityCode, airportCharge);

                // If no city was found, return a 404 Not Found
                if (updatedCity == null)
                {
                    return NotFound(new { Message = $"City with CityCode {cityCode} not found." });  // HTTP 404 Not Found
                }

                // Return the updated city in the response
                return Ok(updatedCity);  // HTTP 200 OK
            }
            catch (KeyNotExistException ex)
            {
                // Handle the case where the city does not exist
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(new { Message = ex.Message });  // HTTP 404 Not Found
            }
            
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });  // HTTP 500 Internal Server Error
            }
        }


        // Get all cities
        
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCitiesAsync()
        {
            try
            {
                // Fetch all cities from the repository
                var cities = await _cityRepository.GetAllDataAsync();

                // Return the list of cities
                return Ok(cities); // HTTP 200 OK
            }
            
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message }); // HTTP 500 Internal Server Error
            }
        }


        // Get a city by CityCode
        [HttpGet("ByCityCode/{cityCode}")]
        public async Task<ActionResult<City>> GetCityByCityCodeAsync(string cityCode)
        {
            try
            {
                // Call the repository to get the city by city code
                var city = await _cityRepository.GetCityByCityCodeAsync(cityCode);

                // If the city is not found, throw a custom exception
                if (city == null)
                {
                    throw new KeyNotExistException($"City with CityCode {cityCode} does not exist.");
                }

                // Return the city if found
                return Ok(city);
            }
            catch (KeyNotExistException ex)
            {
                // Handle the case when the city does not exist
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(new { Message = ex.Message }); // Return a 404 response with the error message
            }
            catch (Exception ex)
            {
                // Handle unexpected errors (e.g., database issues, server errors)
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving the city.", Details = ex.Message }); // Return a 500 response
            }
        }

    }
}
