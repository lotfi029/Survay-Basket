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
            Id = DefaultUsers.AdminId,
            FirstName = "Survay",
            LastName = "Admin",
            Email = DefaultUsers.AdminEmail,
            UserName = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            ConcurrencyStamp = DefaultUsers.ConcurrencyStamp,
            SecurityStamp = DefaultUsers.SecurityStamp,
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEGGazIzGMOSb0LidgAkPx5j3DXx08kxbPRNElSZk855KI7jGHHVK0y3nvo3UxkzH1Q=="
        });
    }
}
  