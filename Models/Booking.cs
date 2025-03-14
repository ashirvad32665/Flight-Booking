using System;
using System.Collections.Generic;

namespace BookingService.Models;

public partial class Booking
{
    public int BookingId { get; set; }
    public string ReferenceNumber { get; set; }
    public int FlightId { get; set; }
    public decimal TotalFare { get; set; }
    public string Status { get; set; }  // Confirmed, Checked-in, Cancelled
    public bool CheckedIn { get; set; }
    public string? CheckInId { get; set; }
    public string? SeatNumber { get; set; }
    public bool IsActive { get; set; }
    public List<Passenger> Passengers { get; set; } = new List<Passenger>();
}
