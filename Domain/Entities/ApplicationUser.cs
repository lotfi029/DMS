namespace Domain.Entities;

public class ApplicationUser : IdentityUser, IAuditable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt {  get; set; } = DateTime.UtcNow;
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    private ApplicationUser() : base() { }

    private ApplicationUser(
        string id,
        string userName, 
        string email, 
        string firstName, 
        string lastName,
        string? phonenumber = null,
        string? securityStamp = null,
        string? concurrencyStamp = null,
        string? passwordHashed = null) : base(userName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        LastLoginAt = DateTime.UtcNow;
        EmailConfirmed = true;
        Email = email;
        NormalizedEmail = email.ToUpper();
        UserName = userName;
        NormalizedUserName = userName.ToUpper();
        PhoneNumberConfirmed = true;
        PhoneNumber = phonenumber;
        SecurityStamp = securityStamp ?? Guid.NewGuid().ToString("D");
        ConcurrencyStamp = concurrencyStamp ?? Guid.NewGuid().ToString("D").ToUpper();
        PasswordHash = passwordHashed;
    }

    public static ApplicationUser Create(
        string id,
        string userName, 
        string email, 
        string firstName, 
        string lastName,
        string? phonenumber = null,
        string? securityStamp = null,
        string? concurrencyStamp = null,
        string? passwordHashed = null)
    {
        return new ApplicationUser(
            id: id,
            userName: userName, 
            email: email, 
            firstName: firstName, 
            lastName: lastName, 
            phonenumber: phonenumber, 
            securityStamp: securityStamp, 
            concurrencyStamp: concurrencyStamp, 
            passwordHashed: passwordHashed);
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}