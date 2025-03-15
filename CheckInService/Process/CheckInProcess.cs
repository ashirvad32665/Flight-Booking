
using CheckInService.Repository;
using System.Text.Json;

namespace CheckInService.Process
{
    public class CheckInProcess
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ICheckIn _checkInRepository;

        public CheckInProcess(HttpClient httpClient, IConfiguration configuration, ICheckIn checkInRepository)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _checkInRepository = checkInRepository;
        }

        public async Task<CheckInResponse> CheckInAsync(CheckInRequest request)
        {
            string bookingServiceUrl = _configuration["ServiceUrls:BookingService"];

            // Verify booking reference from Booking Service
            var bookingResponse = await _httpClient.GetAsync($"{bookingServiceUrl}/api/booking/{request.ReferenceNumber}");
            if (!bookingResponse.IsSuccessStatusCode)
            {
                throw new Exception("Invalid booking reference.");
            }

            return await _checkInRepository.CheckInAsync(request);
        }
        public async Task<CheckInResponse> GetCheckInDetailsAsync(string checkInId)
        {
            return await _checkInRepository.GetCheckInDetailsAsync(checkInId);
        }
    }
}
