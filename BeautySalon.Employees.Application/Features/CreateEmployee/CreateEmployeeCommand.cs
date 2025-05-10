using MediatR;
using BeautySalon.Employees.Domain;

namespace BeautySalon.Employees.Application.Features.CreateEmployee
{
    public record CreateEmployeeCommand
    (
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string[] Skill,
        string status

    ) : IRequest<Employee>;
    
}
