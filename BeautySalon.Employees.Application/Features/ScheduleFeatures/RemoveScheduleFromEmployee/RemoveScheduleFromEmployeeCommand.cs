using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.RemoveScheduleFromEmployee;

public record RemoveScheduleFromEmployeeCommand(Guid EmployeeId, Guid ScheduleId) : IRequest<Employee>;