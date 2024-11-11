using Bookify.Infrastructure.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.Configurations;

internal sealed class AuditConfiguration : IEntityTypeConfiguration<AuditLogEntry>
{
    public void Configure(EntityTypeBuilder<AuditLogEntry> builder)
    {
        builder.ToTable("audit_logs");
        
        builder.HasKey(log => log.Id);
        
        builder.Property(log => log.PrimaryKey).HasColumnType("jsonb");
        builder.Property(log => log.PreviousValues).HasColumnType("jsonb");
        builder.Property(log => log.NewValues).HasColumnType("jsonb");
        builder.Property(log => log.ModifiedProperties).HasColumnType("jsonb");
    }
}