using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Availability : Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    public Employee Employee { get; private set; }
    private Availability()
    {
    }

    private Availability(Guid employeeId, DateTime startTime, DateTime endTime)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public static Availability Create(Guid employeeId, DateTime startTime, DateTime endTime)
    {
        return new Availability(employeeId, startTime, endTime);
    }
    
    public void Update(Guid employeeId, DateTime startTime, DateTime endTime)
    {
        EmployeeId = employeeId;
        StartTime = startTime;
        EndTime = endTime;
    }
}
