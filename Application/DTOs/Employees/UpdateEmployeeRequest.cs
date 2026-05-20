namespace Application.DTOs.Employees;

public sealed record UpdateEmployeeRequest(
    string? FirstName = null,
    string? LastName = null,
    string? JobTitle = null,
    ContractType? ContractType = null,
    string? PhoneNumber = null,
    string? EmergencyContactName = null,
    string? EmergencyContactPhone = null,
    string? Notes = null,
    DateOnly? EndDate = null
);
