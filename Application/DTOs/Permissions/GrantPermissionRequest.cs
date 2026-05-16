namespace Application.DTOs.Permissions;

public sealed record GrantPermissionRequest(
    string TargetUserId,
    string Permission,
    string? Reason = null,
    DateTime? ExpiresAt = null);