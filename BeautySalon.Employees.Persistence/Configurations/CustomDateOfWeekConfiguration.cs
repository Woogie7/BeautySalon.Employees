using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySalon.Employees.Persistence.Configurations;

public class CustomDateOfWeekConfiguration : IEntityTypeConfiguration<CustomDateOfWeek>
{
    public void Configure(EntityTypeBuilder<CustomDateOfWeek> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
        
        var dateOfWeek = Enum.GetValues<CustomDateOfWeekEnum>()
            .Select(r => new CustomDateOfWeek
            {
                Id = (int)r,
                Name = r.ToString()
            });

        builder.HasData(dateOfWeek);
    }
}