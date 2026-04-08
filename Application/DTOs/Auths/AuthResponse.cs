namespace Application.DTOs.Auths;

public sealed record AuthResponse(
    string Token,
    string RefreshToken,
    DateTime Expiration
    );