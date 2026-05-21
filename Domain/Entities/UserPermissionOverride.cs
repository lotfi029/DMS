namespace Domain.Entities;

public sealed class UserPermissionOverride : Entity, IAuditable
{
    public string UserId { get; private set; } = string.Empty;
    public ApplicationUser User { get; private set; } = null!;
    public string Permission { get; private set; } = string.Empty;
    public string? Reason { get; private set; }
    public bool IsGranted { get; private set; }
    public string GrantedById { get; private set; } = string.Empty;
    public DateTime? ExpiresAt { get; private set; }
    private UserPermissionOverride() { }

    public static UserPermissionOverride Grant(
        string userId,
        string permission,
        string grantedById,
        string? reason = null,
        DateTime? expiresAt = null)
    {
        return new UserPermissionOverride
        {
            UserId = userId,
            Permission = permission,
            GrantedById = grantedById,
            IsGranted = true,
            Reason = reason,
            ExpiresAt = expiresAt
        };
    }
    public static UserPermissionOverride Deny(
        string userId,
        string permission,
        string grantedById,
        string? reason = null,
        DateTime? expiresAt = null)
    {
        return new UserPermissionOverride
        {
            UserId = userId,
            Permission = permission,
            IsGranted = false,
            GrantedById = grantedById,
            Reason = reason,
            ExpiresAt = expiresAt,
        };
    }

    public bool IsExpired =>
        ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
}