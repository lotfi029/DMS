namespace Application.DTOs.Auths;

public sealed record LoginRequest(
    string Email,
    string Password
    );
