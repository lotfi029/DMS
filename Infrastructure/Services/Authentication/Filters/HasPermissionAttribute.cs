using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Authentication.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{

}