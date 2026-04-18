namespace Domain.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditable
{
    public string Group { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public static ApplicationRoleClaim Create(
        string roleId,
        string claimType,
        string claimValue,
        string? description = null)
    {

        var textInfo = CultureInfo.CurrentCulture.TextInfo;

        var parts = claimValue.Split(['.', '_'], StringSplitOptions.RemoveEmptyEntries);

        var displayName = textInfo.ToTitleCase(string.Join(" ", parts));


        return new ApplicationRoleClaim
        {
            RoleId = roleId,
            ClaimType = claimType,
            ClaimValue = claimValue,
            Group = textInfo.ToTitleCase(parts.FirstOrDefault() ?? string.Empty),
            DisplayName = displayName,
            Description = description ?? $"Allows the user to perform the '{displayName}' action."
        };
    }
}