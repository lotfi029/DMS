namespace Infrastructure.Persistence.Repositories;

internal sealed class EmployeeRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Employee>(dbContext), IEmployeeRepository
{
}