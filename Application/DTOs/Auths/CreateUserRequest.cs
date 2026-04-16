namespace Application.DTOs.Auths;

public sealed record CreateUserRequest(
    string FirstName,
    string LastName,
    string Password,
    string Email,
    string UserName
    );