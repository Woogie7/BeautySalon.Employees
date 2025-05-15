using BeautySalon.Employees.Application.DTO;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeesByService;

public record GetEmployeesByServiceQuery(Guid ServiceId) : IRequest<IEnumerable<EmployeeDto>>;