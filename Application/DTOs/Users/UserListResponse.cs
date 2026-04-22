namespace Application.DTOs.Users;

public sealed record UserListResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string UserType,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt);

public sealed record DetailedUserResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    bool IsActive,
    string UserType,
    DateTime CreatedAt,
    DateTime LastLoginAt,
    string? PhoneNumber,
    IEnumerable<string> Roles);