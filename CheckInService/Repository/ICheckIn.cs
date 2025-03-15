

namespace CheckInService.Repository
{
    public interface ICheckIn
    {
        Task<CheckInResponse> CheckInAsync(CheckInRequest request);
        Task<CheckInResponse> GetCheckInDetailsAsync(string referenceNumber);
    }
}
