using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(account => account.Id);

        builder.Property(account => account.Id)
            .ValueGeneratedOnAdd();

        builder.Property(account => account.UserId)
            .IsRequired();

        builder.Property(account => account.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(account => account.Type)
            .IsRequired();

        builder.Property(account => account.InitialBalance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(account => account.CurrentBalance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(account => account.Color)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(account => account.IsActive)
            .IsRequired();

        builder.Property(account => account.CreatedAt)
            .IsRequired();

        builder.Property(account => account.UpdatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(account => account.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}