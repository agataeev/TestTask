using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Flight : BaseEntity
{
    [MaxLength(256)]
    public string? Origin { get; set; }
    [MaxLength(256)]
    public string? Destination { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public Status Status { get; set; }

}

public enum Status
{
    OnTime,
    Delayed,
    Departed,
    Arrived,
    Canceled
}