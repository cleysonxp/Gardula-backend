using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class MonthlyBudgetConfiguration : IEntityTypeConfiguration<MonthlyBudget>
{
    public void Configure(EntityTypeBuilder<MonthlyBudget> builder)
    {
        builder.ToTable("MonthlyBudgets");

        builder.HasKey(budget => budget.Id);

        builder.Property(budget => budget.Id)
            .ValueGeneratedOnAdd();

        builder.Property(budget => budget.UserId)
            .IsRequired();

        builder.Property(budget => budget.Year)
            .IsRequired();

        builder.Property(budget => budget.Month)
            .IsRequired();

        builder.Property(budget => budget.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(budget => budget.CreatedAt)
            .IsRequired();

        builder.Property(budget => budget.UpdatedAt)
            .IsRequired();

        builder.HasIndex(budget => new
        {
            budget.UserId,
            budget.Year,
            budget.Month
        })
        .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(budget => budget.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}