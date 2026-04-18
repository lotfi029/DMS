namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<
        ApplicationUser, 
        ApplicationRole, 
        string, 
        IdentityUserClaim<string>, 
        IdentityUserRole<string>, 
        IdentityUserLogin<string>, 
        ApplicationRoleClaim, 
        IdentityUserToken<string>>(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dms");

        modelBuilder.Entity<Department>().ToTable("departments");
        modelBuilder.Entity<AuditLog>().ToTable("audit_logs");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
