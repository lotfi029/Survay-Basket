using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new(){
                Id  = DefaultRoles.AdminRoleId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminConcurrencyStamp,
            },
            new(){
                Id  = DefaultRoles.UserRoleId,
                Name = DefaultRoles.User,
                NormalizedName = DefaultRoles.User.ToUpper(),
                ConcurrencyStamp = DefaultRoles.UserConcurrencyStamp,
                IsDefault = true,
            }
        ]);
    }
}
  