namespace Application.DTOs.Users;

public sealed record CreatedUserResponse(
    string UserName,
    string Email,
    string Password);

public sealed record UserListResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
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
    DateTime CreatedAt,
    DateTime? LastLoginAt);
