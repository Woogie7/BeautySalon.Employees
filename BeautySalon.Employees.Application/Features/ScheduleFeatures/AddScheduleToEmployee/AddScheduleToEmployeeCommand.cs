using BeautySalon.Employees.Application.DTO;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.AddScheduleToEmployee;

public record AddScheduleToEmployeeCommand(
    Guid EmployeeId,
    string DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
) : IRequest<EmployeeDto>;