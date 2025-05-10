using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Skill : Entity
{
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; set; }
    public string Name { get; private set; }

    private Skill() { } // For EF

    private Skill(Guid employeeId, string name)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        Name = name;
    }

    public static Skill Create(Guid employeeId, string name)
        => new Skill(employeeId, name);
}
