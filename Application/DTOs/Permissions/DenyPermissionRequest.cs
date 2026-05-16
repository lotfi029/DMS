namespace Application.DTOs.Permissions;

public sealed record DenyPermissionRequest(
    string TargetUserId,
    string Permission,
    string? Reason = null);
