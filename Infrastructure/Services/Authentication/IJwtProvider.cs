namespace Infrastructure.Services.Authentication;

public interface IJwtProvider
{
    Task<(string token, int expireMinutes)> GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    string? ValidateToken(string token);
}
