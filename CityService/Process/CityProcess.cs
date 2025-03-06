using CityService.Repository;
using CommonUse;

namespace CityService.Process
{
    public class CityProcess 
    {
        private readonly ICity _repo;
        public CityProcess(ICity repo) => _repo = repo;

        public async Task<IEnumerable<City>> GetAllCities()
        {
            return await _repo.GetAllDataAsync();
        }
        public async Task<bool> AddCity(City city)
        {
            if (_repo.GetCityByCityCodeAsync(city.CityCode) != null)
            {
                throw new KeyAlreadyExistsException("This City Code already exists.");
            }
            return await _repo.AddCityAsync(city);
        }
        public async Task<>
    }
}
