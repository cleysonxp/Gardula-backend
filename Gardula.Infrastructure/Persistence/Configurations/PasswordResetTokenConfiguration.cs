using Gardula.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetTokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.Id)
            .ValueGeneratedOnAdd();

        builder.Property(token => token.UserId)
            .IsRequired();

        builder.Property(token => token.TokenHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(token => token.ExpiresAt)
            .IsRequired();

        builder.Property(token => token.CreatedAt)
            .IsRequired();

        builder.Property(token => token.UsedAt);

        builder.HasIndex(token => token.UserId);

        builder.HasIndex(token => token.TokenHash)
            .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}