using Domain.ReadModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class EmployeeProfileViewConfiguration : IEntityTypeConfiguration<EmployeeProfileView>
{
    public void Configure(EntityTypeBuilder<EmployeeProfileView> builder)
    {
        builder.HasNoKey();
        builder.ToView("V_EmployeeProfile", "dms");
    }
}