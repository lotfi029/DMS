namespace Domain.IRepositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(
        Expression<Func<T, bool>> predicate, 
        bool asNoTracking = true, 
        string[]? include = null,
        string[]? filtersKeys = null,
        CancellationToken ct = default);
    Task<T?> GetByIdAsync(
        object id,
        CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(
        bool asNoTracking = true,
        string[]? filtersKeys = null,
        CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate, 
        bool asNoTracking = true, 
        string[]? include = null,
        string[]? filtersKeys = null, 
        CancellationToken ct = default);
    void Add(T entity);
    void Update(T entity);
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Action<UpdateSettersBuilder<T>> setPropertyCalls, CancellationToken ct = default);
    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        string[]? filtersKeys = null,
        CancellationToken ct = default);
    void Add(IEnumerable<T> entities);
}
