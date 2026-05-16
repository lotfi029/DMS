namespace Application.DTOs.Clients;

public sealed record CreateClientRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string Phone,
    string Address,
    string? Notes = null
);
