using CommonUse;
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
                // Check if the city with the same CityCode or CityName already exists
                var existingCity = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityCode == city.CityCode || c.CityName == city.CityName);

                if (existingCity != null)
                {
                    // If city exists, throw a custom exception
                    throw new KeyAlreadyExistsException($"City with the same code or name already exists: {city.CityCode} - {city.CityName}");
                }

                // Add new city
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (KeyAlreadyExistsException ex)
            {
                // If KeysAlreadyExistException is caught, rethrow it
                throw ex;
            }
            catch (Exception ex)
            {
                // Log and throw a general exception for any other errors
                Console.WriteLine($"Error adding city: {ex.Message}");
                throw new Exception("An error occurred while adding the city.", ex);
            }
        }


        // Update city details
        public async Task<City> UpdateCityAsync(string cityCode, City city)
        {
            try
            {
                // Attempt to find the city by code
                var existingCity = await _context.Cities
                    .Where(c => c.CityCode == cityCode)
                    .FirstOrDefaultAsync();

                // If city does not exist, throw custom exception
                if (existingCity == null)
                {
                    throw new KeyNotExistException(cityCode);
                }

                // Add any validation rules here, if needed, before updating
                if (string.IsNullOrEmpty(city.CityName) || city.AirportCharge < 0)
                {
                    throw new InvalidCityDataException("City Name is required and Airport Charge cannot be negative.");
                }

                // Update the existing city with the new data
                existingCity.CityCode = city.CityCode;
                existingCity.CityName = city.CityName;
                existingCity.State = city.State;
                existingCity.AirportCharge = city.AirportCharge;
                existingCity.IsActive = city.IsActive;

                // Save changes
                await _context.SaveChangesAsync();

                // Return updated city
                return existingCity;
            }
            catch (KeyNotExistException ex)
            {
                // Handle specific CityNotFoundException
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow or handle as necessary
            }
            catch (InvalidCityDataException ex)
            {
                // Handle invalid data exception
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow or handle as necessary
            }
            catch (Exception ex)
            {
                // Catch all other exceptions
                Console.WriteLine($"Unexpected error updating city: {ex.Message}");
                return null;
            }
        }


        // Delete a city by CityCode
        public async Task<bool> DeleteCityAsync(string cityCode)
        {
            try
            {
                // Attempt to find the city by CityCode
                var city = await _context.Cities
                    .Where(c => c.CityCode == cityCode)
                    .FirstOrDefaultAsync();

                // If city doesn't exist, throw an exception
                if (city == null)
                {
                    throw new KeyNotExistException($"City with CityCode {cityCode} does not exist.");
                }

                // Set IsActive to false (soft delete)
                city.IsActive = false;
                await _context.SaveChangesAsync();
                return true;  // Return true if update was successful
            }
            catch (KeyNotExistException ex)
            {
                // Rethrow custom exception if city doesn't exist
                throw ex;
            }
            catch (Exception ex)
            {
                // Log unexpected errors and throw a database operation exception
                Console.WriteLine($"Error deleting city: {ex.Message}");
                throw new Exception("An error occurred while attempting to delete the city.", ex);
            }
        }


        // Update airport charge for a specific city
        public async Task<City> UpdateAirportChargeAsync(string cityCode, int airportCharge)
        {
            try
            {
                // Attempt to find the city by CityCode
                var city = await _context.Cities
                    .Where(c => c.CityCode == cityCode)
                    .FirstOrDefaultAsync();

                // If no city is found, throw a KeyNotExistException
                if (city == null)
                {
                    throw new KeyNotExistException($"City with CityCode {cityCode} does not exist.");
                }

                // Update the AirportCharge
                city.AirportCharge = airportCharge;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return the updated city object
                return city;
            }
            catch (KeyNotExistException ex)
            {
                // If the city doesn't exist, rethrow the exception
                throw ex;
            }
            catch (Exception ex)
            {
                // Log unexpected errors and throw a DatabaseOperationException
                Console.WriteLine($"Error updating airport charge: {ex.Message}");
                throw new Exception("An error occurred while updating the airport charge.", ex);
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
                var city = await _context.Cities
                    .Where(c => c.CityCode == cityCode)
                    .FirstOrDefaultAsync();

                if (city == null)
                {
                    // Throw custom exception when city is not found
                    throw new KeyNotExistException($"City with CityCode '{cityCode}' does not exist.");
                }

                return city;
            }
            catch (KeyNotExistException ex)
            {
                // Handle the custom exception here (you can log or take other actions)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error fetching city by city code: {ex.Message}");
                return null;
            }
        }

    }
}
