using CheckInService.Models;
using CheckInService.Repository;
using System.Text.Json;

namespace CheckInService.Process
{
    public class CheckInProcess
    {
        ICheckIn repo;
        private readonly HttpClient http;
        public CheckInProcess(ICheckIn repo, HttpClient http, IConfiguration config)
        {
            this.repo = repo;
            this.http = http;

            //this.http.BaseAddress = new Uri("api:bookingservice"); //to be read from config file
        }
        public async Task<(bool success, string message, string seatAlloted)> CheckInAsync(int bookingId)
        {
            var response = await http.GetAsync("http://localhost:7005/api/bookings/{bookingId}");
            //call the bookingId controller in booking services
            if (!response.IsSuccessStatusCode)
            {
                return (false, "Booking not found", string.Empty);
            }
            var bookingData = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var bookingStatus = bookingData.RootElement.GetProperty("Status").GetString();
            if (bookingStatus == "Checked-In") return (false, "Already Exist", string.Empty);
            var updatedResponse = await http.PutAsync("http://localhost:7005/api/booking/checkin/{bookingId}", null);
            if (!updatedResponse.IsSuccessStatusCode)
            {
                return (false, "Check-in failed", string.Empty);

            }
            var seat = Guid.NewGuid().ToString();
            var checkIn = new CheckIn
            {
                BookingId = bookingId,
                Status = "Check-In",
                IsActive = true,
            };
            bool isSaved = await repo.CheckInAsync(checkIn);
            if (!isSaved)
            {
                return (false, "Failed to check-in in record", string.Empty);
            }
            return (true, "Check-in Successfull", seat);
        }
    }
}
