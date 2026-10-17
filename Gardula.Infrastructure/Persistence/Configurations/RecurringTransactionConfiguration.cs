
using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.ToTable("RecurringTransactions");

        builder.HasKey(recurringTransaction => recurringTransaction.Id);

        builder.Property(recurringTransaction => recurringTransaction.Id)
            .ValueGeneratedOnAdd();

        builder.Property(recurringTransaction => recurringTransaction.UserId)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(recurringTransaction => recurringTransaction.Type)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(recurringTransaction => recurringTransaction.CategoryId)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.PaymentMethod)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.AccountId)
            .IsRequired(false);

        builder.Property(recurringTransaction => recurringTransaction.CardId)
            .IsRequired(false);

        builder.Property(recurringTransaction => recurringTransaction.DueDay)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.StartDate)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.EndDate)
            .IsRequired(false);

        builder.Property(recurringTransaction => recurringTransaction.IsActive)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.CreatedAt)
            .IsRequired();

        builder.Property(recurringTransaction => recurringTransaction.UpdatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(recurringTransaction => recurringTransaction.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(recurringTransaction => recurringTransaction.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(recurringTransaction => recurringTransaction.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Card>()
            .WithMany()
            .HasForeignKey(recurringTransaction => recurringTransaction.CardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(recurringTransaction => new
        {
            recurringTransaction.UserId,
            recurringTransaction.IsActive
        });
    }
}
