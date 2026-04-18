namespace Infrastructure.Persistence.Interceptors;

internal sealed class AuditSaveChangeInterceptor(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuditSaveChangeInterceptor> logger) : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions _jsonOpts =
        new() { WriteIndented = false };

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not ApplicationDbContext dbContext)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditEntries = BuildAuditEntries(dbContext);

        if (auditEntries.Count > 0)
        {
            dbContext.AuditLogs.AddRange(auditEntries);
            logger.LogInformation("Added {Count} audit log entries", auditEntries.Count);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private List<AuditLog> BuildAuditEntries(ApplicationDbContext ctx)
    {
        var userId = httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.Name);
        var ip = httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var entries = new List<AuditLog>();

        foreach (var entry in ctx.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State is not (EntityState.Added
                or EntityState.Modified or EntityState.Deleted))
                continue;

            var entityName = entry.Metadata.ClrType.Name;
            var entityId = GetPrimaryKey(entry);
            var action = MapAction(entry.State);
            var module = ResolveModule(entityName);

            var (oldValues, newValues, changedCols) = BuildChangeSets(entry);

            entries.Add(AuditLog.Create(
                action: action,
                entityName: entityName,
                module: module,
                entityId: entityId,
                userId: userId,
                userName: userName,
                ipAddress: ip,
                oldValues: oldValues,
                newValues: newValues,
                changedColumns: changedCols,
                description: $"{action} on {entityName} [{entityId}]"
            ));
        }

        return entries;
    }
    private static (string? old, string? @new, string? cols) BuildChangeSets(EntityEntry entry)
    {
        string? oldValues = null, newValues = null, changedCols = null;

        if (entry.State == EntityState.Modified)
        {
            var changed = entry.Properties.Where(p => p.IsModified).ToList();
            oldValues = Serialize(changed.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
            newValues = Serialize(changed.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
            changedCols = Serialize(changed.Select(p => p.Metadata.Name));
        }
        else if (entry.State == EntityState.Added)
        {
            newValues = Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
        }
        else if (entry.State == EntityState.Deleted)
        {
            oldValues = Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
        }

        return (oldValues, newValues, changedCols);
    }
    private static AuditAction MapAction(EntityState state) => state switch
    {
        EntityState.Added => AuditAction.Create,
        EntityState.Modified => AuditAction.Update,
        EntityState.Deleted => AuditAction.Delete,
        _ => AuditAction.Read
    };
    private static string? GetPrimaryKey(EntityEntry entry)
    {
        try
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key is null) return null;
            var values = key.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString())
                .ToArray();
            return string.Join(",", values);
        }
        catch { return null; }
    }

    private static string ResolveModule(string entityName) => entityName switch
    {
        AuditEntityNames.User => AuditModules.Users,
        AuditEntityNames.Department => AuditModules.Departments,
        AuditEntityNames.Role => AuditModules.Roles,
        AuditEntityNames.RoleClaim => AuditModules.Permissions,
        _ => AuditModules.System
    };
    private static string Serialize<T>(T value) =>
        JsonSerializer.Serialize(value, _jsonOpts);
}


