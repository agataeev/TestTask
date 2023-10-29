using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "flights_sh");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("bigint").UseIdentityByDefaultColumn();
        builder.Property(x => x.Username).HasMaxLength(256).HasColumnType("character varying(256)");
        builder.Property(x => x.Password).HasMaxLength(256).HasColumnType("character varying(256)");
        builder.Property(x => x.RoleId).HasColumnType("bigint");}
}