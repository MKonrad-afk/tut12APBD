using Microsoft.EntityFrameworkCore;
using tut12.Data;
using tut12.Models;

namespace tut12.repositories;

public class TripRepository : ITripRepository
{
    private readonly MasterContext _context;

    public TripRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> GetTripsAsync(int page, int pageSize)
        => await _context.Trips
            .Include(t => t.Client_Trips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> GetTotalTripCountAsync()
        => await _context.Trips.CountAsync();

    public async Task<Trip?> GetTripByIdAsync(int id)
        => await _context.Trips.FindAsync(id);

    public async Task<bool> IsTripInFutureAsync(int tripId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        return trip != null && trip.DateFrom > DateTime.UtcNow;
    }

    public async Task RegisterClientToTripAsync(Client_Trip registration)
    {
        await _context.Client_Trips.AddAsync(registration);
    }
}