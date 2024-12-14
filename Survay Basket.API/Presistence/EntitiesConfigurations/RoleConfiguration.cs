using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new(){
                Id  = DefaultRoles.Admin.Id,
                Name = DefaultRoles.Admin.Name,
                NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp,
            },
            new(){
                Id  = DefaultRoles.User.Id,
                Name = DefaultRoles.User.Name,
                NormalizedName = DefaultRoles.User.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.User.ConcurrencyStamp,
                IsDefault = true,
            }
        ]);
    }
}
  