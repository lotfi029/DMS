namespace Application.DTOs.Permissions;

public sealed record PermissionResponse(
    int Id,
    string RoleId,
    string Group,
    string Name,
    string DisplayName,
    string? Description
);