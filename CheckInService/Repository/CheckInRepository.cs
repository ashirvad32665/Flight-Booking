
using Microsoft.EntityFrameworkCore;
using Models;

namespace CheckInService.Repository
{
    public class CheckInRepository : ICheckIn
    {
        CheckInDbContext context;
        public CheckInRepository(CheckInDbContext context)
        {
            this.context = context;
        }
        public async Task<CheckInResponse> CheckInAsync(CheckInRequest request)
        {
            // Generate a unique Check-in ID
            string checkInId = "CHK" + new Random().Next(10000, 99999);
            var seatNumber = GenerateSeatNumber();
            var checkIn = new CheckIn
            {
                CheckInId = checkInId,
                ReferenceNumber = request.ReferenceNumber,
                CheckInTime = DateTime.Now,
                SeatNumber = seatNumber
            };

            context.CheckIns.Add(checkIn);
            await context.SaveChangesAsync();

            return new CheckInResponse
            {
                CheckInId = checkIn.CheckInId,
                ReferenceNumber = checkIn.ReferenceNumber,

                SeatNumber = checkIn.SeatNumber,
                CheckInTime = checkIn.CheckInTime
            };
        }

        public async Task<CheckInResponse> GetCheckInDetailsAsync(string checkInId)
        {
            var checkIn = await context.CheckIns.FirstOrDefaultAsync(c => c.CheckInId == checkInId);
            if (checkIn == null) throw new Exception("Check-in not found.");

            return new CheckInResponse
            {
                CheckInId = checkIn.CheckInId,
                ReferenceNumber = checkIn.ReferenceNumber,

                SeatNumber = checkIn.SeatNumber,
                CheckInTime = checkIn.CheckInTime
            };
        }

        private string GenerateSeatNumber()
        {
            Random random = new Random();
            int seat = random.Next(1, 30);
            char row = (char)('A' + random.Next(0, 6)); // A-F rows
            return $"{row}{seat}";
        }
    }
}
