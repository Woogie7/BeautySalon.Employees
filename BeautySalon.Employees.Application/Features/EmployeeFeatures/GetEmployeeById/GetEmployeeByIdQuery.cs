using BeautySalon.Employees.Application.DTO;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDto?>;