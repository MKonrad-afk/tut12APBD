using tut12.DTOs;

namespace tut12.services;

public interface ITripService
{
    Task<object> GetTripsAsync(int page, int pageSize);
    Task<bool> DeleteClientAsync(int idClient);
    Task<string> AssignClientToTripAsync(int idTrip, AddClientRequest request);
}
