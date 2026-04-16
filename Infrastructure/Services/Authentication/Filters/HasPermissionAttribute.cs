using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Authentication.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute(string permission)
    : Attribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public string Permission { get; } = permission;
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}