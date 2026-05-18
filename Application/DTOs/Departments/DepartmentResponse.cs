namespace Application.DTOs.Departments;

public record DepartmentResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid DepartmentHeadId,
    DateTime CreatedAt);
