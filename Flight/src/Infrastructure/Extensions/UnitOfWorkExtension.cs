using System.Diagnostics.CodeAnalysis;
using Domain;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class UnitOfWorkExtension
{
    public static IServiceCollection SetupUnitOfWork([NotNull] this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>(f =>
        {
            var scopeFactory = f.GetRequiredService<IServiceScopeFactory>();
            var context = f.GetService<ApplicationDbContext>();
            return new UnitOfWork(
                context,
                new FlightRepository(context.Flights),
                new UserRepository(context.Users),
                new RoleRepository(context.Roles)
            );
        });
        return serviceCollection;
    }
}
