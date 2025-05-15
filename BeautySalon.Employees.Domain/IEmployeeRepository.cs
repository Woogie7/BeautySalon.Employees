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
        Task<Service> GetServiceByIdAsync(Guid id);
        Task CreateAsync(Employee employee);
        Task CreateServiceAsync(Service service);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
        Task<Employee?> GetByIdWithSchedulesAsync(Guid employeeId);
        Task<IEnumerable<Employee>> GetByServiceIdAsync(Guid serviceId);

    }
}
