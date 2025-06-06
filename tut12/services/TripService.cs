using tut12.DTOs;
using tut12.Models;
using tut12.repositories;

namespace tut12.services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly IClientRepository _clientRepository;

    public TripService(ITripRepository tripRepository, IClientRepository clientRepository)
    {
        _tripRepository = tripRepository;
        _clientRepository = clientRepository;
    }

    public async Task<object> GetTripsAsync(int page, int pageSize)
    {
        var trips = await _tripRepository.GetTripsAsync(page, pageSize);
        var totalCount = await _tripRepository.GetTotalTripCountAsync();
        var allPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var result = new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = allPages,
            trips = trips.Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO { Name = c.Name }).ToList(),
                Clients = t.Client_Trips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToList()
        };

        return result;
    }

    public async Task<bool> DeleteClientAsync(int clientId)
    {
        var client = await _clientRepository.GetClientByIdAsync(clientId);
        if (client == null) return false;

        if (await _clientRepository.HasTripsAsync(clientId))
            return false;

        await _clientRepository.DeleteClientAsync(client);
        await _clientRepository.SaveChangesAsync();
        return true;
    }

    public async Task<string> AssignClientToTripAsync(int idTrip, AddClientRequest request)
    {
        if (await _clientRepository.ExistsByPeselAsync(request.Pesel))
            return "Client with this PESEL already exists.";

        if (await _clientRepository.IsRegisteredToTripAsync(request.Pesel, idTrip))
            return "Client is already registered to this trip.";

        if (!await _tripRepository.IsTripInFutureAsync(idTrip))
            return "Trip has already occurred or does not exist.";

        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Telephone = request.Telephone,
            Pesel = request.Pesel
        };

        var registration = new Client_Trip
        {
            IdTrip = idTrip,
            IdClientNavigation = client,
            PaymentDate = request.PaymentDate,
            RegisteredAt = DateTime.UtcNow
        };

        await _clientRepository.AddClientAsync(client);
        await _tripRepository.RegisterClientToTripAsync(registration);
        await _clientRepository.SaveChangesAsync();

        return "Client registered to trip successfully.";
    }
}