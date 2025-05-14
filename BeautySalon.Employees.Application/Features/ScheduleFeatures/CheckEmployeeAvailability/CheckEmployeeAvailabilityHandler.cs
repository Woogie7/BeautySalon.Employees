using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
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

        return !employee.Schedules.Any(s =>
            s.DateOfWeek == new CustomDateOfWeek { Id = (int)request.DayOfWeek, Name = request.DayOfWeek.ToString() } &&
            s.OverlapsWith(request.StartTime, request.EndTime));
    }
}