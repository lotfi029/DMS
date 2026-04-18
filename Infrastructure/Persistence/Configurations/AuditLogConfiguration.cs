using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs", "dms");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Action)
            .HasConversion<string>()   // store as string for readability
            .HasMaxLength(100);

        builder.Property(x => x.Outcome)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.EntityName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Module).HasMaxLength(100);
        builder.Property(x => x.EntityId).HasMaxLength(100);
        builder.Property(x => x.UserId).HasMaxLength(100);
        builder.Property(x => x.UserName).HasMaxLength(256);
        builder.Property(x => x.UserEmail).HasMaxLength(256);
        builder.Property(x => x.IpAddress).HasMaxLength(50);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.RequestPath).HasMaxLength(2000);
        builder.Property(x => x.RequestMethod).HasMaxLength(10);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.FailureReason).HasMaxLength(2000);

        // JSON columns — no max length
        builder.Property(x => x.OldValues).HasColumnType("text");
        builder.Property(x => x.NewValues).HasColumnType("text");
        builder.Property(x => x.ChangedColumns).HasColumnType("text");

        // Indexes for common query patterns
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.Module);
        builder.HasIndex(x => new { x.EntityName, x.EntityId });
        builder.HasIndex(x => x.Outcome);
    }
}