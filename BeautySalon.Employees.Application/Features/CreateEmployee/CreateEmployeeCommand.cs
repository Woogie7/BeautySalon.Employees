using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    ) : IRequest;
    
}
