using BeautySalon.Employees.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySalon.Employees.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.IsAvailable).IsRequired();

        builder.HasOne(s => s.DateOfWeek)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.DateOfWeekId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Employee)
            .WithMany(e => e.Schedules)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
