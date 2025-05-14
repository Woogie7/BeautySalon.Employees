using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.RemoveScheduleFromEmployee;

public class RemoveScheduleFromEmployeeHandler : IRequestHandler<RemoveScheduleFromEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;

    public RemoveScheduleFromEmployeeHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> Handle(RemoveScheduleFromEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        employee.RemoveSchedule(request.ScheduleId);
        await _employeeRepository.UpdateAsync(employee);

        return employee;
    }
}