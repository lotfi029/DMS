namespace Application.DTOs.Auths;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Password,
    string Email,
    string UserName
    );