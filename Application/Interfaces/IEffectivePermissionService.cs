namespace Application.Interfaces;

public interface IEffectivePermissionService
{
    Task<IEnumerable<string>> ResolveAsync(
        string userId, CancellationToken ct = default);
}