using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await DbContext.Set<T>().SingleOrDefaultAsync(predicate, ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbContext.Set<T>().ToListAsync(ct);
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        var query = await DbContext.Set<T>()
            .Where(predicate)
            .ToListAsync(ct);

        return query;
    }

    public void Add(T entity, CancellationToken ct = default)
    {
        DbContext.Set<T>().Add(entity);
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
    {
        DbContext.Set<T>().Update(entity);
        await DbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await DbContext.Set<T>()
            .Where(predicate)
            .ExecuteDeleteAsync(ct);
    }
    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<T, bool>> predicate,
        Action<UpdateSettersBuilder<T>> setPropertyCalls,
        CancellationToken ct = default)
    {
        return await DbContext.Set<T>()
            .Where(predicate)
            .ExecuteUpdateAsync(setPropertyCalls, ct);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await DbContext.Set<T>().AnyAsync(predicate, ct);
    }
}
