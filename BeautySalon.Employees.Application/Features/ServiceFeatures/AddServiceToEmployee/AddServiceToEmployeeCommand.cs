using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;

public record AddServiceToEmployeeCommand(Guid EmployeeId, Guid ServiceId) : IRequest;
