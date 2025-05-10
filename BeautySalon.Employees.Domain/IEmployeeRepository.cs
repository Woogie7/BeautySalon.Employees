using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Employees.Domain
{
    public interface IEmployeeRepository
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(Guid id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
        
        Task<Employee?> GetByIdWithScheduleAsync(Guid id);
    }
}
