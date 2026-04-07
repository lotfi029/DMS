using Domain.Entities;

namespace Infrastructure.Services.Authentication;

public interface IJwtProvider
{
    Task<(string token, int expireMinutes)> GenerateTokenAsync(ApplicationUser user);
}
