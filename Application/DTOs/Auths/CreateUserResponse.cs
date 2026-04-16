namespace Application.DTOs.Auths;

public sealed record CreateUserResponse(
    string UserId,
    string UserName,
    string Email,
    string Password
);