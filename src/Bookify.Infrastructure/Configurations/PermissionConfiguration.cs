using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Permissions)
            .UsingEntity<Dictionary<string, object>>(
                "role_permissions",
                joinEntity => joinEntity
                    .HasData(
                        new
                        {
                            RolesId = Role.Registered.Id,
                            PermissionsId = Permission.UsersRead.Id
                        })
            );

        builder.HasData(Permission.UsersRead, Permission.UsersWrite);
    }
}