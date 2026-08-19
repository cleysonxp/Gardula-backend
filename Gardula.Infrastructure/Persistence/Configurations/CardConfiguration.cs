using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");

        builder.HasKey(card => card.Id);

        builder.Property(card => card.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(card => card.LastFourDigits)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(card => card.CreditLimit)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(card => card.ClosingDay)
            .IsRequired();

        builder.Property(card => card.DueDay)
            .IsRequired();

        builder.Property(card => card.Brand)
            .IsRequired();

        builder.Property(card => card.Color)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(card => card.IsActive)
            .IsRequired();

        builder.Property(card => card.CreatedAt)
            .IsRequired();

        builder.Property(card => card.UpdatedAt)
            .IsRequired();

        // Account -> Cards
        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(card => card.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Cards
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(card => card.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(card => card.UserId);

        builder.HasIndex(card => card.AccountId);
    }
}