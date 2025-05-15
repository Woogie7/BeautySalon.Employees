namespace BeautySalon.Employees.Application.DTO;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public int DateOfWeekId { get; set; }
    public string DateOfWeekName { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}