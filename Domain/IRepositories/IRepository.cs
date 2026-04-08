namespace Domain.IRepositories;

public interface IRepository<T> where T : Entity
{
    Guid Add(T entity);
    void AddRange(IEnumerable<T> entityList);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Attach(T entity);
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> FindAsync(Guid id, string[]? ignoreQueryFilters = null, CancellationToken ct = default);
    Task<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
    Task<int> ExcuteUpdateAsync(Expression<Func<T, bool>> predicate, Action<UpdateSettersBuilder<T>> action, CancellationToken ct = default);
    Task<IEnumerable<T>> GetWithPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, string[]? queryFilter = null, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, string[]? queryFilter = null, CancellationToken ct = default);
}