namespace Infrastructure.Services;

internal sealed class AuditContextAccessor(IHttpContextAccessor httpContextAccessor) : IAuditContextAccessor
{
    public AuditContext GetCurrent()
    {
        if (httpContextAccessor.HttpContext is not { } ctx)
            return new AuditContext(null, null, null, null, null, null, null);

        var user = ctx.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = user.FindFirstValue(ClaimTypes.Name);
        var email = user.FindFirstValue(ClaimTypes.Email);

        var ip = ctx.Connection.RemoteIpAddress?.ToString()
                 ?? ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        var agent = ctx.Request.Headers.UserAgent.ToString();
        var path = ctx.Request.Path.Value;
        var method = ctx.Request.Method;

        return new AuditContext(userId, userName, email, ip, agent, path, method);
    }
}
