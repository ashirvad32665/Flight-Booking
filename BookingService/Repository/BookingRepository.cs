using BookingService.Models;
using CommonUse;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository
{
    public class BookingRepository :IBooking
    {
        private readonly BookingDbContext context;
        public BookingRepository(BookingDbContext context)
        {
            this.context = context;
        }
        public async Task<string> BookFlightAsync(BookingRequest request, decimal totalFare)
        {
            try
            {
                string referenceNumber = "AB2025" + new Random().Next(1000, 9999);

                var booking = new Booking
                {
                    ReferenceNumber = referenceNumber,
                    
                    FlightId = request.FlightId,
                    TotalFare = totalFare,
                    Status = "Confirmed",
                    Passengers = request.Passengers.Select(p => new Passenger
                    {
                        FullName = p.Name,
                        Email = p.Email,
                        Gender= p.Gender
                    }).ToList()
                };

                context.Bookings.Add(booking);
                await context.SaveChangesAsync();

                return referenceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving booking: " + ex.Message);
            }
        }

        public async Task<BookingDetail> GetBookingDetailsAsync(string referenceNumber)
        {
            var booking = await context.Bookings
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.ReferenceNumber == referenceNumber);

            if (booking == null)
                throw new Exception("Booking not found.");

            return new BookingDetail
            {
                ReferenceNumber = booking.ReferenceNumber,
                TotalFare = booking.TotalFare,
                Passengers = booking.Passengers.Select(p => new PassengerDTO
                {
                    Name = p.FullName,
                    Email = p.Email,
                    Gender = p.Gender
                }).ToList()
            };
        }
        //public async Task<bool> AddPassenger(Passenger passenger)
        //{
        //    context.Passengers.Add(passenger);
        //    return await context.SaveChangesAsync()>0;
        //} 
    }
}
