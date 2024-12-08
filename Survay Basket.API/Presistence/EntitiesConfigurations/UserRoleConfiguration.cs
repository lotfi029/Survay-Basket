using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>
        {
            RoleId = DefaultRoles.AdminRoleId,
            UserId = DefaultUsers.AdminId,
        });
    }
}
  