using Microsoft.EntityFrameworkCore;

namespace CityService.Repository
{
    public class CityRepository : ICity
    {
        private readonly CityDbContext _context;

        public CityRepository(CityDbContext context)
        {
            _context = context;
        }

        // Add a new city
        public async Task<bool> AddCityAsync(City city)
        {
            try
            {
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding city: {ex.Message}");
                return false;
            }
        }

        // Update city details
        public async Task<City> UpdateCityAsync(int cityId, City city)
        {
            try
            {
                var existingCity = await _context.Cities
                    .Where(c => c.CityId == cityId)
                    .FirstOrDefaultAsync();

                if (existingCity == null)
                    return null;

                existingCity.CityCode = city.CityCode;
                existingCity.CityName = city.CityName;
                existingCity.State = city.State;
                existingCity.AirportCharge = city.AirportCharge;
                existingCity.IsActive = city.IsActive;

                await _context.SaveChangesAsync();
                return existingCity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating city: {ex.Message}");
                return null;
            }
        }

        // Delete a city by ID
        public async Task<bool> DeleteCityAsync(int cityId)
        {
            try
            {
                var city = await _context.Cities
                    .Where(c => c.CityId == cityId)
                    .FirstOrDefaultAsync();

                if (city == null)
                    return false;

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting city: {ex.Message}");
                return false;
            }
        }

        // Update airport charge for a specific city
        public async Task<City> UpdateAirportChargeAsync(int cityId, int airportCharge)
        {
            try
            {
                var city = await _context.Cities
                    .Where(c => c.CityId == cityId)
                    .FirstOrDefaultAsync();

                if (city == null)
                    return null;

                city.AirportCharge = airportCharge;
                await _context.SaveChangesAsync();
                return city;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating airport charge: {ex.Message}");
                return null;
            }
        }

        // Get all cities
        public async Task<IEnumerable<City>> GetAllDataAsync()
        {
            try
            {
                return await _context.Cities.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cities: {ex.Message}");
                return Enumerable.Empty<City>();
            }
        }

        // Get a city by CityCode
        public async Task<City> GetCityByCityCodeAsync(string cityCode)
        {
            try
            {
                return await _context.Cities
                    .Where(c => c.CityCode == cityCode)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching city by city code: {ex.Message}");
                return null;
            }
        }

    }
}
