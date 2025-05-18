using BeautySalon.Employees.Domain;

namespace BeautySalon.Employees.Persistence.Repository;

public interface IScheduleRepository
{
    Task<Schedule> GetByIdAsync(Guid id);
    Task CreateAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Schedule schedule);
    Task SaveChangesAsync();
}
