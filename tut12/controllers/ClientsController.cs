using Microsoft.AspNetCore.Mvc;
using tut12.services;

namespace tut12.controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ITripService _service;

    public ClientsController(ITripService service)
    {
        _service = service;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var deleted = await _service.DeleteClientAsync(idClient);
        return deleted ? NoContent() : BadRequest("Cannot delete client with active trips or client not found.");
    }
}
