using tut12.Models;

namespace tut12.repositories;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int id);
    Task<bool> HasTripsAsync(int clientId);
    Task DeleteClientAsync(Client client);
    Task<bool> ExistsByPeselAsync(string pesel);
    Task<bool> IsRegisteredToTripAsync(string pesel, int tripId);
    Task AddClientAsync(Client client);
    Task SaveChangesAsync();
}