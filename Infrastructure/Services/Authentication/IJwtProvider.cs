namespace Infrastructure.Services.Authentication;

public interface IJwtProvider
{
    (string token, int expireMinutes) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
