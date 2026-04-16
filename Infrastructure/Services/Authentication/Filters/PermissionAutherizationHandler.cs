using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Authentication.Filters;

internal sealed class PermissionAutherizationHandler : AuthorizationHandler<HasPermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        HasPermissionAttribute requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(e => e.Value == requirement.Permission && e.Type == DefaultPermissions.ClaimType))
            return;

        context.Succeed(requirement);

        return;
    }
}
