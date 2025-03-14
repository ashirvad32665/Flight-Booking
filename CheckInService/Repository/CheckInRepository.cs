using CheckInService.Models;

namespace CheckInService.Repository
{
    public class CheckInRepository : ICheckIn
    {
        CheckInDbContext context;
        public CheckInRepository(CheckInDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> CheckInAsync(CheckIn checkin)
        {
            //if (checkin == null) throw new ArgumentNullException();
            //var res=await context.CheckIns.FirstOrDefaultAsync(c=>c.CheckInId == checkin.CheckInId);
            //if (res != null) throw new Exception("Already Exist");
            context.CheckIns.Add(checkin);
            await context.SaveChangesAsync();
            return true;
        }
        //public async Task<CheckIn> UpdateStatus(int bookingId)
        //{
        //    var res=await context.CheckIns.FirstOrDefaultAsync(c=>c.BookingId== bookingId);
        //    if (res is null) throw new IdNotFoundException("Id not found");
        //    res.Status = "Check In Successfully";
        //    await context.SaveChangesAsync();
        //    return res;
        //}
    }
}
