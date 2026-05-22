namespace Application.DTOs.Roles;

public sealed record RoleListResponse(
    string Id,
    string Name,
    bool IsActive
);