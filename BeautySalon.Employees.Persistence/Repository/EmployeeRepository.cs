using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Employees.Persistence.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _dbContext;
        public EmployeeRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException();

            await _dbContext.Employees.AddAsync(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
            }
        }

        public async Task<Employee?> GetByIdWithScheduleAsync(Guid id)
        {
            return await _dbContext.Employees
                .Include(e => e.Schedules)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbContext.Employees
                .Include(e => e.Skills)
                .Include(e => e.Schedules)
                .Include(e => e.Status)
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employees
                .Include(e => e.Skills)
                .Include(e => e.Schedules)
                .Include(e => e.Status)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

        public Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            return Task.CompletedTask;
        }
    }
}
