using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FareService.Repository
{
    public class FareRepository : IFare
    {
        private readonly FareDbContext _context;

        public FareRepository(FareDbContext context)
        {
            _context = context;
        }

        // Method to add a new fare
        public async Task<bool> AddFareForFlight(Fare fare)
        {
            try
            {
                _context.Fares.Add(fare);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while adding the fare.", ex);
            }
        }

        // Method to remove a fare
        public async Task<bool> RemoveFareForFlight(int fareId)
        {
            try
            {
                var fare = await _context.Fares.FindAsync(fareId);
                if (fare == null) return false;

                _context.Fares.Remove(fare);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while removing the fare.", ex);
            }
        }

        // Method to update fare by fareId
        public async Task<Fare> UpdateFareByFareId(int fareId, decimal basePrice, decimal convenienceFee)
        {
            try
            {
                var fare = await _context.Fares.FindAsync(fareId);
                if (fare == null) return null;

                fare.BasePrice = basePrice;
                fare.ConvenienceFee = convenienceFee;
                await _context.SaveChangesAsync();
                return fare;
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while updating the fare by FareId.", ex);
            }
        }

        // Method to update fare by flightId
        public async Task<Fare> UpdateFareByFlightId(int flightId, decimal basePrice, decimal convenienceFee)
        {
            try
            {
                var fare = await _context.Fares.FirstOrDefaultAsync(f => f.FlightId == flightId);
                if (fare == null) return null;

                fare.BasePrice = basePrice;
                fare.ConvenienceFee = convenienceFee;
                await _context.SaveChangesAsync();
                return fare;
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while updating the fare by FlightId.", ex);
            }
        }

        // Method to get fare by FlightId
        public async Task<Fare> GetFareByFlightId(int flightId)
        {
            try
            {
                return await _context.Fares
                                      .FirstOrDefaultAsync(f => f.FlightId == flightId);
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while retrieving the fare by FlightId.", ex);
            }
        }

        // Method to get fare by FareId
        public async Task<Fare> GetFareByFareID(int fareId)
        {
            try
            {
                return await _context.Fares
                                      .FirstOrDefaultAsync(f => f.FareId == fareId);
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while retrieving the fare by FareId.", ex);
            }
        }

        // Method to get the total fare (BasePrice + ConvenienceFee)
        public async Task<decimal> GetTotalFare(int fareId)
        {
            try
            {
                var fare = await _context.Fares.FindAsync(fareId);
                if (fare == null)
                {
                    throw new Exception("Fare not found");
                }

                return fare.BasePrice + fare.ConvenienceFee;
            }
            catch (Exception ex)
            {
                // Log the exception if needed (optional)
                throw new Exception("An error occurred while calculating the total fare.", ex);
            }
        }
    

    }
}
