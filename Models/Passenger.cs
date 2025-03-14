using System;
using System.Collections.Generic;


namespace BookingService.Models;

public partial class Passenger
{
    
    public int PassengerId { get; set; }
    public int BookingId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? SeatNumber { get; set; }
    public string Gender { get; set; }
    //public string? PassportNumber { get; set; }
    public bool IsActive { get; set; }
}
