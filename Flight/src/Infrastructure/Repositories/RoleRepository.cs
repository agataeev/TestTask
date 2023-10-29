using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository: Repository<Role>, IRoleRepository
{
    public RoleRepository(DbSet<Role> entities) : base(entities)
    {
    }
}