using System.Security.Cryptography;

namespace Infrastructure.Services;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext dbContext,
    IJwtProvider jwtProvider,
    IAuditService auditService,
    IHttpContextAccessor httpContextAccessor
    ) : IAuthService
{
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<string>> RegisterAsync(string roleId, CreateUserRequest request, CancellationToken ct = default)
    {
        if (await userManager.Users.AnyAsync(e => e.Email == request.Email, ct))
            return AuthErrors.DuplicatedEmail(request.Email);

        var user = ApplicationUser.Create(
            id: Guid.CreateVersion7().ToString(),
            userName: request.UserName,
            email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName
        );

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Error.BadRequest("Auth.Register", string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(roleId))
        {
            if (await roleManager.FindByIdAsync(roleId) is not { } role)
                return RoleErrors.NotFound;

            var roleResult = await userManager.AddToRoleAsync(user, role.Name!);
            if (!roleResult.Succeeded)
                return Error.BadRequest("Auth.Register", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }
        else
        {
            var roleResult = await userManager
                .AddToRoleAsync(user, DefaultRoles.Employee.Name!);
            
            if (!roleResult.Succeeded)
                return Error.BadRequest("Auth.Register", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return Result.Success(user.Id);
    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var userId = jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Error.Unauthorized("Auth.RefreshToken", "Invalid Token");

        if (await userManager.FindByIdAsync(userId) is not { } user)
            return Error.Unauthorized("Auth.RefreshToken", "Invalid User Id");


        if (!user.IsActive)
            return Error.Unauthorized(userId, "User Not Active");


        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == request.RefreshToken && e.IsActive);


        if (userRefreshToken is null)
            return Error.Unauthorized("Auth.RefreshToken", "Invalid Refresh Token");

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        var (roles, permission) = await GetUserRoleAndClaims(user, ct);
        var (newToken, expiresIn) = jwtProvider.GenerateToken(user, roles, permission);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await userManager.UpdateAsync(user);
        await auditService.LogAsync(AuditLog.TokenRefreshed(user.Id, IpAddress()), ct);

        return new AuthResponse(newToken, newRefreshToken, DateTime.UtcNow.AddMinutes(expiresIn));
    }
    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken ct = default)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            await auditService.LogAsync(
                AuditLog.Login(string.Empty, request.Email, request.Email, IpAddress(), false,
                    "Invalid credentials"), ct);
            return Error.Unauthorized("Auth.Login", "Invalid Credintionals");
        }

        if (!user.IsActive)
        {
            await auditService.LogAsync(
                AuditLog.Login(string.Empty, request.Email, request.Email, IpAddress(), false,
                    "Invalid credentials"), ct);
            return Error.Unauthorized("Auth.Login", "User Not Active");
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await auditService.LogAsync(
                AuditLog.Login(string.Empty, request.Email, request.Email, IpAddress(), false,
                    "Invalid credentials"), ct);
            return Error.Unauthorized("Auth.Login", "Invalid Credintionals");
        }

        var (roles, permissions) = await GetUserRoleAndClaims(user, ct);
        var (token, expireMinutes) = jwtProvider.GenerateToken(user, roles, permissions);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        user.UpdateLastLogin();
        await userManager.UpdateAsync(user);
        await auditService.LogAsync(
            AuditLog.Login(
                userId: user.Id,
                userName: user.UserName!,
                email: user.Email!,
                ip: IpAddress(),
                success: true
                ),
            ct);

        return new AuthResponse(token, refreshToken, DateTime.UtcNow.AddMinutes(expireMinutes));
    }

    public async Task<Result> RevokeRefreshToken(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var userId = jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Error.BadRequest("Auth.RevokeRefreshToken", "Invalid Token");

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Error.BadRequest("Auth.RevokeRefreshToken", "Invalid User Id");

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == request.RefreshToken && e.IsActive);

        if (userRefreshToken is null)
            return Error.BadRequest("Auth.RevokeRefreshToken", "No Refresh Token");

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);
        await auditService.LogAsync(AuditLog.TokenRevoked(user.Id, IpAddress()), ct);

        return Result.Success();
    }
    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRoleAndClaims(ApplicationUser user, CancellationToken ct = default)
    {
        var roles = await userManager.GetRolesAsync(user);

        var permission = await (
            from r in dbContext.Roles
            join c in dbContext.RoleClaims
            on r.Id equals c.RoleId
            where roles.Contains(r.Name!)
            select c.ClaimValue)
            .Distinct()
            .ToListAsync(ct);

        return (roles, permission);
    }
    private string? IpAddress()
    {
        if (httpContextAccessor.HttpContext is not { } ctx)
            return null;

        return ctx.Connection.RemoteIpAddress?.ToString()
                 ?? ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();

    }
}