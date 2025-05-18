using BeautySalon.Employees.Domain;

namespace BeautySalon.Employees.Persistence.Repository;

public interface IScheduleRepository
{
    Task CreateAsync(Schedule service);
    Task SaveChangesAsync();
}