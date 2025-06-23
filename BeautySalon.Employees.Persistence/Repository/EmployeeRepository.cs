using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Persistence.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _dbContext;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(EmployeeDBContext dbContext, ILogger<EmployeeRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Service> GetServiceByIdAsync(Guid id)
        {
            return await _dbContext.Services
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException();

            await _dbContext.Employees.AddAsync(employee);
        }

        public async Task CreateServiceAsync(Service service)
        {
            if (service == null) throw new ArgumentNullException();

            await _dbContext.Services.AddAsync(service);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
            }
        }

        public async Task<Employee?> GetByIdWithSchedulesAsync(Guid employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.Schedules)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<IEnumerable<Employee>> GetByServiceIdAsync(Guid serviceId)
        {
            return await _dbContext.Employees
                .Include(e => e.Skills)
                .Where(e => e.Skills.Any(s => s.ServiceId == serviceId))
                .Include(e => e.Availabilities)
                .AsNoTracking()
                .ToListAsync();
        }

        
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            _logger.LogInformation("Getting all employees");
            return await _dbContext.Employees
                .Where(e => e.IsActive)
                .Include(e => e.Skills)
                .ThenInclude(s => s.Service)
                .Include(e => e.Schedules)
                .Include(e => e.Availabilities)
                .AsSplitQuery()
                .ToListAsync();
        }


        public async Task<Employee> GetByIdAsync(Guid id)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Skills)
                .ThenInclude(s => s.Service)
                .Include(e => e.Schedules)
                .Include(e => e.Availabilities)
                .FirstOrDefaultAsync(e => e.Id == id);

            return employee?.IsActive == true ? employee : null;
        }



        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

        // public Task UpdateAsync(Employee employee)
        // {
        //     _dbContext.Employees.Update(employee);
        //     return Task.CompletedTask;
        // }
    }
}
