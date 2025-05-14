using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.CheckEmployeeAvailability;

public record CheckEmployeeAvailabilityCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}