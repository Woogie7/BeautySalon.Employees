using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetAllEmployees;

public class GetEmployeeHandler: IRequestHandler<GetEmployeesQuery, IEnumerable<Employee>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public Task<IEnumerable<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        return _employeeRepository.GetAllAsync();
    }
}