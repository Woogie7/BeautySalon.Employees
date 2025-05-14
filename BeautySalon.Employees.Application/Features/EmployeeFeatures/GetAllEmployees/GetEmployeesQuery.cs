using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetAllEmployees;

public record GetEmployeesQuery() : IRequest<IEnumerable<Employee>>;