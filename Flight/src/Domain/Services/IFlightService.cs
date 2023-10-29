using Domain.Common;
using Domain.DTOs;

namespace Domain.Services;

public interface IFlightService
{
    Task<FlightDto> AddAsync(FlightDto flightDto);
    Task UpdateAsync(FlightDto flightDto);
    Task DeleteAsync(long flightId);
    Task<FlightDto> GetByIdAsync(long id);
    Task<IEnumerable<FlightDto>> GetAllAsync();
    Task<IEnumerable<FlightDto>> GetAllAsync(FlightFilter filter);
}