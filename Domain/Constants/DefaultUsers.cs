namespace Domain.Constants;

public static class DefaultUsers
{
    // DefaultPassword123!

    public static readonly ApplicationUser Manager = ApplicationUser.Create(
        id: "d1f8c9e2-5b6a-4c3e-9a1b-2f8e7c9d0a1b",
        userName: "manager",
        email: "manager@system.dms",
        firstName: "Default",
        lastName: "Manager",
        passwordHashed: "AQAAAAIAAYagAAAAEO+viSpwZfDcerDVZKFkj8KyR/DTccUyEY4rkX2+ju0KwGZiT6acd1W7clgf9WSjfw=="
        );

    public static readonly IList<ApplicationUser> All = [Manager];
}
