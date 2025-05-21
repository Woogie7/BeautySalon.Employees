namespace BeautySalon.Employees.Application.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<ServiceDto> Services { get; set; }
    public List<ScheduleDto> Schedules { get; set; }
}