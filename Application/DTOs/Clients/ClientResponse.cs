namespace Application.DTOs.Clients;

public sealed record ClientResponse(
    Guid Id,
    string AppUserId,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    bool IsActive,
    string? PhoneNumber,
    string? Address,
    string? Notes
);
