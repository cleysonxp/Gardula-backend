using Gardula.Domain.Entities.Authentication;
using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class FinancialGoalConfiguration
    : IEntityTypeConfiguration<FinancialGoal>
{
    public void Configure(EntityTypeBuilder<FinancialGoal> builder)
    {
        builder.ToTable("FinancialGoals");

        builder.HasKey(goal => goal.Id);

        builder.Property(goal => goal.Id)
            .ValueGeneratedOnAdd();

        builder.Property(goal => goal.UserId)
            .IsRequired();

        builder.Property(goal => goal.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(goal => goal.Description)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(goal => goal.CurrentAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(goal => goal.TargetAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(goal => goal.TargetDate)
            .IsRequired();

        builder.Property(goal => goal.Icon)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(goal => goal.CreatedAt)
            .IsRequired();

        builder.Property(goal => goal.UpdatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(goal => goal.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}