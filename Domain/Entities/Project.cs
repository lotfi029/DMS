namespace Domain.Entities;

public sealed class Project : Entity, IAuditable
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public ProjectStatus Status { get; private set; }

    public Guid ClientId { get; private set; }
    public string? CreatedById { get; private set; }
    public ApplicationUser? ApplicationUser { get; private set; }


    private Project() { }
    private Project(
        string name,
        ProjectStatus status,
        Guid clientId,
        string? createdById,
        string? description) : base()
    {
        Name = name;
        Status = status;
        ClientId = clientId;
        CreatedById = createdById;
        Description = description;
    }

    public static Project Create(
        string name,
        ProjectStatus status,
        Guid clientId,
        string? createdById,
        string? description = null)
    {
        return new(name, status, clientId, createdById, description);
    }

    public void UpdateStatus(ProjectStatus status)
        => Status = status;
}
public enum ProjectStatus
{
    InProgress = 1, Pinding, NotStarted,
    Done
}