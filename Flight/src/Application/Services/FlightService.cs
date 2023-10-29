using System.Linq.Expressions;
using AutoMapper;
using Domain;
using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;

namespace Application.Services;

public class FlightService : IFlightService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<FlightDto> AddAsync(FlightDto flightDto)
    {
        var flight = _mapper.Map<Flight>(flightDto);
        
        await _unitOfWork.FlightRepository.AddAsync(flight);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FlightDto>(flight);
    }
    
    public async Task UpdateAsync(FlightDto flightDto)
    {
        var exists = await _unitOfWork.FlightRepository.ExistAsync(x => x.Id == flightDto.Id);
        if (!exists)
            throw new ConflictException("The flight doesn't exist");

        var flight = _mapper.Map<Flight>(flightDto);
        await _unitOfWork.FlightRepository.UpdateAsync(flight);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task DeleteAsync(long flightId)
    {
        var exists = await _unitOfWork.FlightRepository.ExistAsync(x => x.Id == flightId);
        if (!exists)
            throw new NotFoundException("The flight doesn't exist.");

        var flight = await _unitOfWork.FlightRepository.GetByIdAsync(flightId);
        await _unitOfWork.FlightRepository.DeleteAsync(flight);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<FlightDto> GetByIdAsync(long id)
    {
        var flight = await _unitOfWork.FlightRepository.GetByIdAsync(id);
        return _mapper.Map<FlightDto>(flight);
    }
    
    public async Task<IEnumerable<FlightDto>> GetAllAsync()
    {
        var flights = await _unitOfWork.FlightRepository.GetAllAsync();
        flights = flights.OrderBy(x => x.Arrival);
        return _mapper.Map<IEnumerable<FlightDto>>(flights);
    }
    
    public async Task<IEnumerable<FlightDto>> GetAllAsync(FlightFilter filter)
    {
        var flights = await _unitOfWork.FlightRepository.GetAllAsync();

        if (filter.Origin != null)
        {
            flights = flights.Where(x => x.Origin == filter.Origin);
        }

        if (filter.Destination != null)
        {
            flights = flights.Where(x => x.Destination == filter.Destination);
        }

        flights = flights.OrderBy(x => x.Arrival);
    
        return _mapper.Map<IEnumerable<FlightDto>>(flights);
    }
}