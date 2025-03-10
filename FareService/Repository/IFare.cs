namespace FareService.Repository
{
    public interface IFare
    {
        Task<bool> AddFareForFlight(Fare fare);
        Task<bool> RemoveFareForFlight(int fareId);
        Task<Fare> UpdateBasePriceByFareId(int fareId);
        Task<Fare> UpdateBasePriceByFlightId(int flightId);
        Task<Fare> GetFareByFlightId(int flightId);
        Task<Fare> GetFareByFareID(int fareId);


    }
}
