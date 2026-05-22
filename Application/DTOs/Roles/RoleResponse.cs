namespace Application.DTOs.Roles;

public sealed record RoleResponse(
    string Id,
    string Name,
    string? Description,
    bool IsActive,
    int UserCount,
    int PermissionCount,
    DateTime CreatedAt
    );
