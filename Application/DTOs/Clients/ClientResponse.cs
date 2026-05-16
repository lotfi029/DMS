namespace Application.DTOs.Clients;

public sealed record ClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    bool IsActive,
    string? PhoneNumber,
    string? Address,
    string? Notes
);
