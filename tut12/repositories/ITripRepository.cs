using tut12.Models;

namespace tut12.repositories;

public interface ITripRepository
{
    Task<List<Trip>> GetTripsAsync(int page, int pageSize);
    Task<int> GetTotalTripCountAsync();
    Task<Trip?> GetTripByIdAsync(int id);
    Task<bool> IsTripInFutureAsync(int tripId);
    Task RegisterClientToTripAsync(Client_Trip registration);
}