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

    public async Task CreateAsync(Schedule service)
    {
        await _context.Schedules.AddAsync(service);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}