namespace CityService.Repository
{
    public interface ICity
    {
        Task<bool> AddCityAsync(City city);
        Task<City> UpdateCityAsync(int cityId, City city);
        Task<bool> DeleteCityAsync(int cityId);
        Task<City> UpdateAirportChargeAsync(int cityId, int airportCharge);
        Task<IEnumerable<City>> GetAllDataAsync();
        Task<City> GetCityByCityCodeAsync(string cityCode);
    }
}
