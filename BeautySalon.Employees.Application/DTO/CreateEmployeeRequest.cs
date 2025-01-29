using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Employees.Application.DTO
{
    public record CreateEmployeeRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string[] Skill,
        string status

    );
}
