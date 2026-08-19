using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
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

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Card> Cards => Set<Card>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Transfer> Transfers => Set<Transfer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GardulaDbContext).Assembly);
    }
}