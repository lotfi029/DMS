namespace Application.DTOs.Clients;

public sealed record UpdateClientRequest(
    string AppUserId,
    string? FirstName = null,
    string? LastName = null,
    string? Address = null,
    string? Phone = null,
    string? Notes = null) : ICommand;