using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetAllEmployees;

public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeDto>>;