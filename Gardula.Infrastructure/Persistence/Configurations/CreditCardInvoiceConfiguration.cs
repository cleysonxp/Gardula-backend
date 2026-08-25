using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class CreditCardInvoiceConfiguration
    : IEntityTypeConfiguration<CreditCardInvoice>
{
    public void Configure(
        EntityTypeBuilder<CreditCardInvoice> builder)
    {
        builder.ToTable("CreditCardInvoices");

        builder.HasKey(invoice => invoice.Id);

        builder.Property(invoice => invoice.Id)
            .ValueGeneratedOnAdd();

        builder.Property(invoice => invoice.UserId)
            .IsRequired();

        builder.Property(invoice => invoice.CardId)
            .IsRequired();

        builder.Property(invoice => invoice.StartDate)
            .IsRequired();

        builder.Property(invoice => invoice.ClosingDate)
            .IsRequired();

        builder.Property(invoice => invoice.DueDate)
            .IsRequired();

        builder.Property(invoice => invoice.Status)
            .IsRequired();

        builder.Property(invoice => invoice.PaidAt)
            .IsRequired(false);

        builder.Property(invoice => invoice.CreatedAt)
            .IsRequired();

        builder.Property(invoice => invoice.UpdatedAt)
            .IsRequired();

        // User -> Credit Card Invoices
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(invoice => invoice.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Card -> Credit Card Invoices
        builder.HasOne<Card>()
            .WithMany()
            .HasForeignKey(invoice => invoice.CardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(invoice => invoice.UserId);

        builder.HasIndex(invoice => invoice.CardId);

        builder.HasIndex(invoice => new
        {
            invoice.CardId,
            invoice.StartDate,
            invoice.ClosingDate
        });
    }
}