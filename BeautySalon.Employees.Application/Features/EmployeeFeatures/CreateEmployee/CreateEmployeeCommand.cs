using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee
{
    public record CreateEmployeeCommand
    (
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        List<Guid> ServiceIds

    ) : IRequest<EmployeeDto>;
    
}
