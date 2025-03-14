using System;
using System.Collections.Generic;

namespace CheckInService.Models;

public partial class CheckIn
{
    public int CheckInId { get; set; }

    public int BookingId { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }
}
