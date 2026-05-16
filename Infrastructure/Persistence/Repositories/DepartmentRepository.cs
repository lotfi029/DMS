namespace Infrastructure.Persistence.Repositories;

internal sealed class DepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
}
