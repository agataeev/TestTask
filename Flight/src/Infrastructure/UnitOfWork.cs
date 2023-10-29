using Domain;
using Domain.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    // public IPermissionRepository PermissionRepository { get; set; }
    // public IPermissionTypeRepository PermissionTypeRepository { get; set; }

    private readonly ApplicationDbContext _dbContext;
    public IFlightRepository FlightRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    
    public IRoleRepository RoleRepository { get; set; }

    public UnitOfWork(ApplicationDbContext dbContext, FlightRepository flightRepository, UserRepository userRepository, IRoleRepository roleRepository)
    {
        _dbContext = dbContext;
        FlightRepository = flightRepository;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    public async Task<int> SaveAsync() => await _dbContext.SaveChangesAsync();

    public void Dispose() => _dbContext.Dispose();
}
