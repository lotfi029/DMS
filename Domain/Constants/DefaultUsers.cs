using Domain.Entities;

namespace Domain.Constants;

public static class DefaultUsers
{
    public static readonly ApplicationUser Manager = new()
    {
        Id = "d1f8c9e2-5b6a-4c3e-9a1b-2f8e7c9d0a1b",
        UserName = "manager",
        NormalizedUserName = "MANAGER",
        Email = "manager@system.dms",
        NormalizedEmail = "manager@system.dms".ToUpper(),
        FirstName = "Default",
        LastName = "Manager",
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        LastLoginAt = DateTime.UtcNow,
        EmailConfirmed = true,
        PasswordHash = "AQAAAAIAAYagAAAAEFt3Bi8L7046PSKyRz381Zz0/Z5kr1G8TIRkvy5ruDIsRS2boNa8as3FyzmW5YN/mw==",
        ConcurrencyStamp = Guid.NewGuid().ToString(),
        PhoneNumberConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString()
    };


    public static readonly IList<ApplicationUser> All = [Manager];
}
