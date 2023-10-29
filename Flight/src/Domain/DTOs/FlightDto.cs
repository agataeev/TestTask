using Domain.Entities;

namespace Domain.DTOs;

public class FlightDto
{
    public long Id { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public Status Status { get; set; }
}