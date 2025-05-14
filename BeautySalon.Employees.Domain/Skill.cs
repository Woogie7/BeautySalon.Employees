using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Skill : Entity
{
    public Guid EmployeeId { get; private set; }
    public Guid ServiceId { get; private set; }

    public Employee Employee { get; private set; }
    public Service Service { get; private set; }

    private Skill() { }

    private Skill(Guid employeeId, Guid serviceId)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        ServiceId = serviceId;
    }

    public static Skill Create(Guid employeeId, Guid serviceId)
        => new Skill(employeeId, serviceId);
}

