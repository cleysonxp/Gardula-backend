using Gardula.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gardula.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

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

        builder.Property(token => token.RevokedAt);

        builder.HasIndex(token => token.UserId)
            .IsUnique();

        builder.HasIndex(token => token.TokenHash)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<RefreshToken>(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}