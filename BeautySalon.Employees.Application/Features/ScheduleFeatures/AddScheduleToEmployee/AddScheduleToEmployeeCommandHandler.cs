using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.AddScheduleToEmployee;

public class AddScheduleToEmployeeCommandHandler : IRequestHandler<AddScheduleToEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;

    public AddScheduleToEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> Handle(AddScheduleToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var schedule = Schedule.Create(
            Guid.NewGuid(),
            request.DayOfWeek,
            request.StartTime,
            request.EndTime
        );

        if (employee.Schedules.Any(s => s.OverlapsWith(schedule.StartTime, schedule.EndTime)))
        {
            throw new InvalidOperationException("The new schedule overlaps with an existing one.");
        }
        
        employee.AddSchedule(schedule);

        await _employeeRepository.UpdateAsync(employee);

        return employee;
    }
}
