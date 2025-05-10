namespace BeautySalon.Employees.Application.DTO;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public int DateOfWeek { get; set; } // Можно использовать перечисление CustomDateOfWeekEnum
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public ScheduleDto(Guid id, int dateOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        Id = id;
        DateOfWeek = dateOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}