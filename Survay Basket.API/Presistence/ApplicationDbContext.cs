using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Survay_Basket.API.Presistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor
    ) : IdentityDbContext<ApplicationUser,ApplicationRole, string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public virtual DbSet<Poll> Polls { get; set; } = default!;
    public virtual DbSet<Question> Questions { get; set; } = default!;
    public virtual DbSet<Answer> Answers { get; set; } = default!;
    public virtual DbSet<Vote> Votes { get; set; } = default!;
    public DbSet<VoteAnswer> VoteAnswers { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    // audiat
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

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

