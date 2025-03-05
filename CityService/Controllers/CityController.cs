using CityService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityRepository _cityRepository;

        public CityController(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        // Add a new city
        [HttpPost("Add")]
        public async Task<bool> AddCityAsync(City city)
        {
            return await _cityRepository.AddCityAsync(city);
        }

        // Update city details
        [HttpPut("update")]
        public async Task<City> UpdateCityAsync(int cityId, City city)
        {
            return await _cityRepository.UpdateCityAsync(cityId, city);
        }

        // Delete a city by ID
        [HttpDelete("Delete/{cityId}")]
        public async Task<bool> DeleteCityAsync(int cityId)
        {
            return await _cityRepository.DeleteCityAsync(cityId);
        }

        // Update airport charge for a city
        [HttpPut("UpdateCharge")]
        public async Task<City> UpdateAirportChargeAsync(int cityId, int airportCharge)
        {
            return await _cityRepository.UpdateAirportChargeAsync(cityId, airportCharge);
        }

        // Get all cities
        [HttpGet("All")]
        public async Task<IEnumerable<City>> GetAllDataAsync()
        {
            return await _cityRepository.GetAllDataAsync();
        }

        // Get a city by CityCode
        [HttpGet("ByCityCode/{cityCode}")]
        public async Task<City> GetCityByCityCodeAsync(string cityCode)
        {
            return await _cityRepository.GetCityByCityCodeAsync(cityCode);
        }
    }
}
