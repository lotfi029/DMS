namespace Application.DTOs.Permissions;

public sealed record RevokePermissionRequest(
    string TargetUserId,
    string Permission
    );