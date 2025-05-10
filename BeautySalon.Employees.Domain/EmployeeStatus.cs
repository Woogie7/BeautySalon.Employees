namespace BeautySalon.Employees.Domain;

public class EmployeeStatus
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; }
}