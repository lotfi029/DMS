using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;
    public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
    {
        return await DbContext.Set<T>()
            .FindAsync([id], ct);
    }
    public async Task<T?> GetByIdAsync(
        Expression<Func<T, bool>> predicate, 
        bool asNoTracking = true,
        string[]? include = null, 
        string[]? filtersKeys = null,
        CancellationToken ct = default)
    {
        var query = DbContext.Set<T>().AsQueryable();

        if (include != null)
        {
            foreach (var navigationProperty in include)
            {
                query = query.Include(navigationProperty);
            }
        }

        if (filtersKeys is not null)
        {
            query.IgnoreQueryFilters(filtersKeys);
        }

        if (asNoTracking)
            query.AsNoTracking();



        return await query.SingleOrDefaultAsync(predicate, ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        bool asNoTracking = true,
        string[]? filtersKeys = null,
        CancellationToken ct = default)
    {
        var query = DbContext.Set<T>()
            .IgnoreAutoIncludes();

        if (filtersKeys is not null)
            query.IgnoreQueryFilters(filtersKeys); 

        if (asNoTracking)
            query.AsNoTracking();

        return await query.ToListAsync(ct);
    }
    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate, 
        bool asNoTracking = true, 
        string[]? include = null,
        string[]? filterKeys = null, 
        CancellationToken ct = default)
    {
        var query = DbContext.Set<T>().AsQueryable()
            .IgnoreAutoIncludes();

        if (include != null)
        {
            foreach (var navigationProperty in include)
            {
                query = query.Include(navigationProperty);
            }
        }
        if (filterKeys is not null)
            query.IgnoreQueryFilters(filterKeys);

        if (asNoTracking)
            query.AsNoTracking();

        return await query.Where(predicate).ToListAsync(ct);
    }

    public void Add(T entity)
    {
        DbContext.Set<T>().Add(entity);
    }
    public void Add(IEnumerable<T> entities)
    {
        DbContext.Set<T>().AddRange(entities);
    }

    public void Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }
    public void Remove(T entity)
    {
        DbContext.Set<T>().Remove(entity);
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

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        string[]? filtersKeys = null,
        CancellationToken ct = default)
    {
        var query = DbContext.Set<T>()
            .IgnoreAutoIncludes();

        if (filtersKeys != null)
            query.IgnoreQueryFilters(filtersKeys);

        return await query.AnyAsync(predicate, ct);
    }
}