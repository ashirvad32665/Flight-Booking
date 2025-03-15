namespace CheckInService.Repository
{
    public class CheckInResponse
    {
        public string CheckInId { get; set; }
        public string ReferenceNumber { get; set; }
        public string SeatNumber { get; set; }
        public DateTime? CheckInTime { get; set; }
    }
    public class CheckInRequest
    {
        public string ReferenceNumber { get; set; } // Booking reference number
    }
}
