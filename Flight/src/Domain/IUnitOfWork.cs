using Domain.Repositories;

namespace Domain;

public interface IUnitOfWork : IDisposable
{
    IFlightRepository FlightRepository { get; set; }
    IUserRepository UserRepository { get; set; }
    
    IRoleRepository RoleRepository { get; set; }
    Task<int> SaveAsync();
}
