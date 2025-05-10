using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BeautySalon.Employees.Persistence.Context;

public class EmployeeDBContextFactory : IDesignTimeDbContextFactory<EmployeeDBContext>
{
    public EmployeeDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EmployeeDBContext>();
        optionsBuilder.UseNpgsql("Host=employees-service-postgres;Port=5432;Database=BeautySalonBookingDb;Username=postgres;Password=1234");

        return new EmployeeDBContext(optionsBuilder.Options);
    }
}