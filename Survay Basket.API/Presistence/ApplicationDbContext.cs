using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Survay_Basket.API.Presistence.EntitiesConfigurations;
using System.Reflection;
using System.Security.Claims;

namespace Survay_Basket.API.Presistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor
    ) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public virtual DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        foreach (var entityTrack in entries)
        {
            if (entityTrack.State == EntityState.Added)
            {
                entityTrack.Property(e => e.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityTrack.State == EntityState.Modified)
            {
                entityTrack.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                entityTrack.Property(e => e.UpdateOn).CurrentValue = DateTime.UtcNow;

            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

