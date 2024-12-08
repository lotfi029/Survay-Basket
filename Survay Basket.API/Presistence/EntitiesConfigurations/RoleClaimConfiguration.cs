using MimeKit.Tnef;
using Survay_Basket.API.Abstractions.Consts;
using System.Diagnostics;

namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permissions = Permissions.GetAllPermissions;
        var adminClaim = new List<IdentityRoleClaim<string>>();
        var roleId = DefaultRoles.AdminRoleId;
        int cnt = 0;

        foreach(var permission in permissions)
        {
            adminClaim.Add(new()
            {
                Id = ++cnt,
                ClaimType = Permissions.Type,
                ClaimValue = permission,
                RoleId = roleId
            });
        }

        builder.HasData(adminClaim);
    }
}
