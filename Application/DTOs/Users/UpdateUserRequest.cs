namespace Application.DTOs.Users;

public sealed record UpdateUserRequest(
     string FirstName,
     string LastName
    );