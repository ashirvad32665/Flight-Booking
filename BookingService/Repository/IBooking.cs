using BookingService.Models;

namespace BookingService.Repository
{
    public interface IBooking
    {
        Task<string> BookFlightAsync(BookingRequest request, decimal totalFare);
        Task<BookingDetail> GetBookingDetailsAsync(string referenceNumber);
    }
}
