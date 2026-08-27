using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class CreditCardInvoicePaymentConfiguration
    : IEntityTypeConfiguration<CreditCardInvoicePayment>
{
    public void Configure(
        EntityTypeBuilder<CreditCardInvoicePayment> builder)
    {
        builder.ToTable("CreditCardInvoicePayments");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id)
            .ValueGeneratedOnAdd();

        builder.Property(payment => payment.UserId)
            .IsRequired();

        builder.Property(payment => payment.CreditCardInvoiceId)
            .IsRequired();

        builder.Property(payment => payment.AccountId)
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(payment => payment.PaymentMethod)
            .IsRequired();

        builder.Property(payment => payment.PaidAt)
            .IsRequired();

        builder.Property(payment => payment.CreatedAt)
            .IsRequired();

        builder.Property(payment => payment.UpdatedAt)
            .IsRequired();

        // User -> Credit Card Invoice Payments
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(payment => payment.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Credit Card Invoice -> Payments
        builder.HasOne<CreditCardInvoice>()
            .WithMany()
            .HasForeignKey(payment => payment.CreditCardInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Account -> Payments
        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(payment => payment.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(payment => payment.UserId);

        builder.HasIndex(payment => payment.CreditCardInvoiceId);

        builder.HasIndex(payment => payment.AccountId);
    }
}