namespace Application.DTOs.Permissions;

public sealed record UpdatePermissionRequest(
    string OldPermissionName,
    string NewPermissionName
);

