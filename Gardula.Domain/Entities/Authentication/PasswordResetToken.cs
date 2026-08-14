namespace Gardula.Domain.Entities.Authentication;

public class PasswordResetToken
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UsedAt { get; private set; }

    public PasswordResetToken(
        int userId,
        string tokenHash,
        DateTimeOffset expiresAt)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException(
                "Token hash is required.",
                nameof(tokenHash));

        if (expiresAt <= DateTimeOffset.UtcNow)
            throw new ArgumentException(
                "Token expiration must be in the future.",
                nameof(expiresAt));

        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= ExpiresAt;
    }

    public bool IsUsed()
    {
        return UsedAt.HasValue;
    }

    public bool IsActive()
    {
        return !IsExpired() && !IsUsed();
    }

    public void MarkAsUsed()
    {
        if (IsUsed())
            return;

        UsedAt = DateTimeOffset.UtcNow;
    }
}