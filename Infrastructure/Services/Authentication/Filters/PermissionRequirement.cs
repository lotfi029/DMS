using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Authentication.Filters;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
