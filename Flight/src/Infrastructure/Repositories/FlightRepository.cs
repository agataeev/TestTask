using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FlightRepository : Repository<Flight>, IFlightRepository
{
    public FlightRepository(DbSet<Flight> contextFlights) : base(contextFlights)
    {
    }
}