using Microsoft.AspNetCore.Mvc;
using tut12.DTOs;
using tut12.services;

namespace tut12.controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _service;

    public TripsController(ITripService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AddClientRequest request)
    {
        var result = await _service.AssignClientToTripAsync(idTrip, request);
        if (result == "OK") return Ok();
        return BadRequest(result);
    }
}


