using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Application.Features.AddScheduleToEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.AddScheduleToEmployee;

public class AddScheduleToEmployeeCommandHandler : IRequestHandler<AddScheduleToEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    public AddScheduleToEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(AddScheduleToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);
        
        var dayOfWeek = Domain.SeedWork.Enumeration.FromDisplayName<CustomDateOfWeek>(request.DayOfWeek);
        
        if (dayOfWeek == null)
            throw new NotFoundException("Day of week not found");
        
        var schedule = Schedule.Create(
            Guid.NewGuid(),
            dayOfWeek,
            request.StartTime,
            request.EndTime
        );

        if (employee.Schedules.Any(s => s.OverlapsWith(schedule.StartTime, schedule.EndTime)))
        {
            throw new InvalidOperationException("The new schedule overlaps with an existing one.");
        }
        
        employee.AddSchedule(schedule);
        await _employeeRepository.SaveChangesAsync();

        return _mapper.Map<EmployeeDto>(employee);
    }
}
