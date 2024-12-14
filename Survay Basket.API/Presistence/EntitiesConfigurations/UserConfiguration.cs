using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Presistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(e => e.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");
        
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(1500);

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        
        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.Admin.Id,
            FirstName = "Survay",
            LastName = "Admin",
            Email = DefaultUsers.Admin.Email,
            UserName = DefaultUsers.Admin.Email,
            NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
            NormalizedUserName = DefaultUsers.Admin.Email.ToUpper(),
            ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
            SecurityStamp = DefaultUsers.Admin.SecurityStamp,
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEGGazIzGMOSb0LidgAkPx5j3DXx08kxbPRNElSZk855KI7jGHHVK0y3nvo3UxkzH1Q=="
        });
    }
}
  