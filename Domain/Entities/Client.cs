namespace Domain.Entities;

public sealed class Client : Entity, IAuditable
{
    public string AppUserId { get; set; } = string.Empty;
    public ApplicationUser AppUser { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
    public string? Notes { get; set; }
    private Client() { }
    private Client(
        string userId,
        string address,
        string? notes = null) : base()
    {
        AppUserId = userId;
        Address = address;
        Notes = notes;
    }
    public static Client Create(
        string userId,
        string address,
        string? notes = null)
    {
        return new Client(
            userId: userId,
            address: address,
            notes: notes);
    }
}