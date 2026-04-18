using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Persistence.Interceptors;

internal sealed class AuditSaveChangeInterceptor(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuditSaveChangeInterceptor> logger) : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions JsonOpts =
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
            var action = entry.State switch
            {
                EntityState.Added => AuditAction.Create,
                EntityState.Modified => AuditAction.Update,
                EntityState.Deleted => AuditAction.Delete,
                _ => AuditAction.Read
            };

            string? oldValues = null;
            string? newValues = null;
            string? changedCols = null;

            if (entry.State == EntityState.Modified)
            {
                var changed = entry.Properties
                    .Where(p => p.IsModified)
                    .ToList();

                oldValues = JsonSerializer.Serialize(
                    changed.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue),
                    JsonOpts);
                newValues = JsonSerializer.Serialize(
                    changed.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue),
                    JsonOpts);
                changedCols = JsonSerializer.Serialize(
                    changed.Select(p => p.Metadata.Name), JsonOpts);
            }
            else if (entry.State == EntityState.Added)
            {
                newValues = JsonSerializer.Serialize(
                    entry.Properties.ToDictionary(
                        p => p.Metadata.Name, p => p.CurrentValue),
                    JsonOpts);
            }
            else if (entry.State == EntityState.Deleted)
            {
                oldValues = JsonSerializer.Serialize(
                    entry.Properties.ToDictionary(
                        p => p.Metadata.Name, p => p.OriginalValue),
                    JsonOpts);
            }

            entries.Add(AuditLog.Create(
                action: action,
                entityName: entityName,
                module: ResolveModule(entityName),
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
        "ApplicationUser" => "Users",
        "ApplicationRole" => "Roles",
        "Department" => "Departments",
        "ApplicationRoleClaim" => "RoleClaims",
        _ => "System"
    };

}

public interface IAuditable { }
