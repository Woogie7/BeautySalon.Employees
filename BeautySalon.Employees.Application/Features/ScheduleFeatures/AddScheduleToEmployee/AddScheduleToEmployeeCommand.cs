using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;

namespace BeautySalon.Employees.Application.Features.AddScheduleToEmployee;

public record AddScheduleToEmployeeCommand(
    Guid EmployeeId,
    CustomDateOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
) : IRequest<Employee>;