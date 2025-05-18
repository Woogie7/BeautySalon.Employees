using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Persistence.Repository;

public class ServiceRepository : IServiceRepository
{
    private readonly EmployeeDBContext _context;

    public ServiceRepository(EmployeeDBContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid serviceId)
    {
        return await _context.Services.AnyAsync(s => s.Id == serviceId);
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Service>> GetAllAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task CreateAsync(Service service)
    {
        await _context.Services.AddAsync(service);
    }

    public async Task DeleteAsync(Guid id)
    {
        var service = await _context.Services.FindAsync(id);
        
        if (service is not null)
        {
            var skills = await _context.Skills.Where(s => s.ServiceId == service.Id).ToListAsync();
            _context.Skills.RemoveRange(skills);
            _context.Services.Remove(service);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
