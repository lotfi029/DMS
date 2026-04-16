namespace Application.DTOs.Users;

public sealed record AddUserRequest(
     string FirstName,
     string LastName,
     string Password,
     string Email,
     string UserName,
     string? RoleId,
     Guid? DepartmentId
    );




public sealed record UpdateUserRequest(
     string FirstName,
     string LastName
    );