using Microsoft.EntityFrameworkCore;
using tut12.Data;
using tut12.Models;

namespace tut12.repositories;

public class ClientRepository : IClientRepository
{
    private readonly MasterContext _context;

    public ClientRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetClientByIdAsync(int id)
        => await _context.Clients.FindAsync(id);

    public async Task<bool> HasTripsAsync(int clientId)
        => await _context.Client_Trips.AnyAsync(ct => ct.IdClient == clientId);

    public async Task DeleteClientAsync(Client client)
    {
        _context.Clients.Remove(client);
    }

    public async Task<bool> ExistsByPeselAsync(string pesel)
        => await _context.Clients.AnyAsync(c => c.Pesel == pesel);

    public async Task<bool> IsRegisteredToTripAsync(string pesel, int tripId)
        => await _context.Client_Trips
            .AnyAsync(ct => ct.IdTrip == tripId && ct.IdClientNavigation.Pesel == pesel);

    public async Task AddClientAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}