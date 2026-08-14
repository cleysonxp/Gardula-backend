namespace Gardula.Domain.Entities.Authentication;

public class RefreshToken
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? RevokedAt { get; private set; }

    public RefreshToken(
        int userId,
        string tokenHash,
        DateTimeOffset expiresAt)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("Token hash is required.", nameof(tokenHash));

        if (expiresAt <= DateTimeOffset.UtcNow)
            throw new ArgumentException("Token expiration must be in the future.", nameof(expiresAt));

        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= ExpiresAt;
    }

    public bool IsRevoked()
    {
        return RevokedAt.HasValue;
    }

    public bool IsActive()
    {
        return !IsExpired() && !IsRevoked();
    }

    public void Revoke()
    {
        if (IsRevoked())
            return;

        RevokedAt = DateTimeOffset.UtcNow;
    }
}