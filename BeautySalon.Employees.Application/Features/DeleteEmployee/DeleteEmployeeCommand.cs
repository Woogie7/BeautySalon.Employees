using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest<Employee?>;
