namespace Domain.Users;

public class RefreshToken
{
    public RefreshTokenId Id { get; }
    public UserId UserId { get; }
    public User User { get;}
    public string Token { get; private set; }
    public DateTime Expires { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime RevokedAt { get; private set; }
    public bool IsActive => DateTime.UtcNow < Expires && RevokedAt == null;
    
    private RefreshToken(RefreshTokenId id, UserId userId, string token, DateTime expires, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedAt = createdAt;
    }
    
    public static RefreshToken New(RefreshTokenId id, UserId userId, string token, DateTime expires)
        => new(id, userId, token, expires, DateTime.UtcNow);

    public void UpdateToken(string token, DateTime expires)
    {
        Token = token;
        Expires = expires;
    }
    
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
    
}