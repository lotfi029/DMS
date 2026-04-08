namespace Application.DTOs.Auths;

public sealed record RefreshTokenRequest(
    string Token,
    string RefreshToken
    );
