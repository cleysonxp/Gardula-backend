using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Id)
            .ValueGeneratedOnAdd();

        builder.Property(transaction => transaction.UserId)
            .IsRequired();

        builder.Property(transaction => transaction.AccountId)
            .IsRequired(false);

        builder.Property(transaction => transaction.CardId)
            .IsRequired(false);

        builder.Property(transaction => transaction.CategoryId)
            .IsRequired(false);

        builder.Property(transaction => transaction.TransferId)
            .IsRequired(false);

        builder.Property(transaction => transaction.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(transaction => transaction.Type)
            .IsRequired();

        builder.Property(transaction => transaction.PaymentMethod)
            .IsRequired();

        builder.Property(transaction => transaction.Description)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(transaction => transaction.Date)
            .IsRequired();

        builder.Property(transaction => transaction.InstallmentGroupId)
            .IsRequired(false);

        builder.Property(transaction => transaction.InstallmentNumber)
            .IsRequired(false);

        builder.Property(transaction => transaction.TotalInstallments)
            .IsRequired(false);

        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired();

        builder.Property(transaction => transaction.UpdatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(transaction => transaction.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(transaction => transaction.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Card>()
            .WithMany()
            .HasForeignKey(transaction => transaction.CardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Transfer>()
            .WithMany()
            .HasForeignKey(transaction => transaction.TransferId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}