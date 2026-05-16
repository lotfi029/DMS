namespace Infrastructure.Persistence.Repositories;

internal sealed class EmployeeDepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<EmployeeDepartment>(dbContext), IEmployeeDepartmentRepository;