using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Domain.SeedWork;
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

        builder
            .Property(b => b.DateOfWeek)
            .HasConversion(
                v => v.Name,
                v => Enumeration.FromDisplayName<CustomDateOfWeek>(v)
            )
            .IsRequired();

        builder.HasOne(s => s.Employee)
            .WithMany(e => e.Schedules)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
