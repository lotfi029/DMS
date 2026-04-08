namespace Application.DTOs.Users;

public sealed record UserResponse(
    string UserName,
    string Email,
    string Password);
