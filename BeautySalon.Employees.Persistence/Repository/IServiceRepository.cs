using BeautySalon.Employees.Domain;

namespace BeautySalon.Employees.Persistence.Repository;

public interface IServiceRepository
{
    Task<bool> ExistsAsync(Guid serviceId);
    Task<Service?> GetByIdAsync(Guid id);
    Task<List<Service>> GetAllAsync();
    Task CreateAsync(Service service);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}