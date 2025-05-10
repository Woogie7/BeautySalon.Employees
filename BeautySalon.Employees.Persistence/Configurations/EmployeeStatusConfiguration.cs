using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySalon.Employees.Persistence.Configurations;

public class EmployeeStatusConfiguration : IEntityTypeConfiguration<EmployeeStatus>
{
    public void Configure(EntityTypeBuilder<EmployeeStatus> builder)
    {
        builder.HasKey(es => es.Id);
        builder.Property(es => es.Name).IsRequired().HasMaxLength(50);
        
        var statuses = Enum.GetValues<EmployeeStatusEnum>()
            .Select(r => new EmployeeStatus
            {
                Id = (int)r,
                Name = r.ToString()
            });

        builder.HasData(statuses);
    }
}
