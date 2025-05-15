using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using BeautySalon.Employees.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalon.Employees.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration confing)
        {
            var сonnection = confing.GetConnectionString("EmployeesDatabase");
            services.AddDbContext<EmployeeDBContext>(o =>
            {
                o.UseNpgsql(сonnection);
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }
    }
}
