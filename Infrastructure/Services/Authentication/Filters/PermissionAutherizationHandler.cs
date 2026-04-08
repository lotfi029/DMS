using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Authentication.Filters;

public class PermissionAutherizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(e => e.Value == requirement.Permission && e.Type == Permissions.ClaimType))
            return;

        context.Succeed(requirement);

        return;
    }
}
