using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest<EmployeeDto?>;
