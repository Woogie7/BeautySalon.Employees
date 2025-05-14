using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.UpdateScheduleForEmployee;

public class UpdateScheduleForEmployeeHandler : IRequestHandler<UpdateScheduleForEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateScheduleForEmployeeHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> Handle(UpdateScheduleForEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var schedule = employee.Schedules.FirstOrDefault(s => s.Id == request.ScheduleId);
        if (schedule == null)
            throw new NotFoundException(nameof(Schedule), request.ScheduleId);

        schedule.UpdateSchedule(new CustomDateOfWeek { Id = (int)request.DayOfWeek, Name = request.DayOfWeek.ToString() }, request.StartTime, request.EndTime);
        await _employeeRepository.UpdateAsync(employee);

        return employee;
    }
}