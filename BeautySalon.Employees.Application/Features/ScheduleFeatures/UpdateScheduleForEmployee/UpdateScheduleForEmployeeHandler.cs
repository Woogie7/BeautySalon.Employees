using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Schedule;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.UpdateScheduleForEmployee;

public class UpdateScheduleForEmployeeHandler : IRequestHandler<UpdateScheduleForEmployeeCommand, Employee>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEventBus _eventBus;

    public UpdateScheduleForEmployeeHandler(IScheduleRepository scheduleRepository, IEmployeeRepository employeeRepository, IEventBus eventBus)
    {
        _scheduleRepository = scheduleRepository;
        _employeeRepository = employeeRepository;
        _eventBus = eventBus;
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
        
        var tempSchedule = Schedule.Create(
            schedule.Id,
            dayOfWeek,
            request.StartTime,
            request.EndTime
        );
        
        var hasOverlap = employee.Schedules
            .Where(s => s.Id != request.ScheduleId)
            .Any(s => s.OverlapsWith(tempSchedule.StartTime, tempSchedule.EndTime));

        if (hasOverlap)
            throw new InvalidOperationException("Updated schedule overlaps with existing schedules.");
        
        schedule.UpdateSchedule(dayOfWeek, request.StartTime, request.EndTime);
        await _scheduleRepository.UpdateAsync(schedule);
        await _scheduleRepository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new ScheduleUpdatedEmployeeEvent(
            employee.Id,
            schedule.Id,
            schedule.DateOfWeekId,
            schedule.StartTime,
            schedule.EndTime
        ));
        
        return employee;
    }

}