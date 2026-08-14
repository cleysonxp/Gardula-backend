using Gardula.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Persistence;

public class GardulaDbContext : DbContext
{
    public GardulaDbContext(DbContextOptions<GardulaDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GardulaDbContext).Assembly);
    }
}