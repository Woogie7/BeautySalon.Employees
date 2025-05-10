using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Availability : Entity
{
    public Guid EmployeeId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public bool IsBusy { get; private set; }

    private Availability() { }

    public Availability(Guid employeeId, DateTime startTime, DateTime endTime, bool isBusy)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        StartTime = startTime;
        EndTime = endTime;
        IsBusy = isBusy;
    }

    public void MarkBusy() => IsBusy = true;
    public void MarkFree() => IsBusy = false;
}
