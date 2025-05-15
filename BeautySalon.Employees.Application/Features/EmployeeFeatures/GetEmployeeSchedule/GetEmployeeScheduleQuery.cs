using BeautySalon.Employees.Application.DTO;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeeSchedule;

public record GetEmployeeScheduleQuery(Guid EmployeeId) : IRequest<IEnumerable<ScheduleDto>>;