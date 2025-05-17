using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.CheckEmployeeAvailability;

public class CheckEmployeeAvailabilityHandler : IRequestHandler<CheckEmployeeAvailabilityCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CheckEmployeeAvailabilityHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> Handle(CheckEmployeeAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var dayOfWeek = Domain.SeedWork.Enumeration.FromDisplayName<CustomDateOfWeek>(request.DayOfWeek);
        
        if (dayOfWeek == null)
            throw new NotFoundException("Day of week not foasdnd");
        
        return !employee.Schedules.Any(s =>
            s.DateOfWeek == dayOfWeek &&
            s.OverlapsWith(request.StartTime, request.EndTime));
    }
}