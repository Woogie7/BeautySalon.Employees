using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.UpdateScheduleForEmployee;

public record UpdateScheduleForEmployeeCommand : IRequest<Employee>
{
    public Guid EmployeeId { get; set; }
    public Guid ScheduleId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}