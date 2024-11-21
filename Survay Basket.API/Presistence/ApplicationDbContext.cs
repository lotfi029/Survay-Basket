using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Survay_Basket.API.Presistence.EntitiesConfigurations;
using System.Reflection;

namespace Survay_Basket.API.Presistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
    ) : IdentityDbContext<ApplicationUser>(options)
{
    public virtual DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

