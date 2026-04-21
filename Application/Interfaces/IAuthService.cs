namespace Application.Interfaces;

public interface IAuthService
{
    // TODO: add register method and remove from user service
    public Task<Result<string>> RegisterAsync(string roleId, RegisterRequest request, CancellationToken ct = default);
    public Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken ct = default);
    public Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    public Task<Result> RevokeRefreshToken(RefreshTokenRequest request, CancellationToken ct = default);
}