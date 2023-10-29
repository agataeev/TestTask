using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database;

public class FlightEntityTypeConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights", "flights_sh");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("bigint").UseIdentityByDefaultColumn();
        builder.Property(x => x.Origin).HasMaxLength(256).HasColumnType("character varying(256)");
        builder.Property(x => x.Destination).HasMaxLength(256).HasColumnType("character varying(256)");
        builder.Property(x => x.Departure).HasColumnType("timestamp with time zone");
        builder.Property(x => x.Arrival).HasColumnType("timestamp with time zone");
        builder.Property(x => x.Status).HasColumnType("integer");
    }
}