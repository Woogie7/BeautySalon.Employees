using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
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
        var dayOfWeek = Domain.SeedWork.Enumeration.FromDisplayName<CustomDateOfWeek>(request.DayOfWeek);
        
        if (dayOfWeek == null)
            throw new NotFoundException("Day of week not found");
        
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var schedule = employee.Schedules.FirstOrDefault(s => s.Id == request.ScheduleId);
        if (schedule == null)
            throw new NotFoundException(nameof(Schedule), request.ScheduleId);

        schedule.UpdateSchedule(dayOfWeek, request.StartTime, request.EndTime);
        await _employeeRepository.SaveChangesAsync();

        return employee;
    }
}