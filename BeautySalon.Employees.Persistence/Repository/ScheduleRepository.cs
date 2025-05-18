using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;

namespace BeautySalon.Employees.Persistence.Repository;

public class ScheduleRepository : IScheduleRepository
{
    private readonly EmployeeDBContext _context;

    public ScheduleRepository(EmployeeDBContext context)
    {
        _context = context;
    }

    public async Task<Schedule> GetByIdAsync(Guid id)
    {
        return await _context.Schedules.FindAsync(id);
    }

    public async Task CreateAsync(Schedule schedule)
    {
        await _context.Schedules.AddAsync(schedule);
    }

    public Task UpdateAsync(Schedule schedule)
    {
        _context.Schedules.Update(schedule);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Schedule schedule)
    {
        _context.Schedules.Remove(schedule);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
