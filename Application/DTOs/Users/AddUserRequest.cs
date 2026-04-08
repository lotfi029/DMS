namespace Application.DTOs.Users;

public sealed record AddUserRequest(
     string FirstName,
     string LastName,
     string Password,
     string Email,
     string UserName
    );
