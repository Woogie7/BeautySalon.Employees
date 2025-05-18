using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Schedule;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.AddScheduleToEmployee;

public class AddScheduleToEmployeeCommandHandler : IRequestHandler<AddScheduleToEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddScheduleToEmployeeCommandHandler> _logger;
    private readonly IEventBus _eventBus;
    public AddScheduleToEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<AddScheduleToEmployeeCommandHandler> logger, IScheduleRepository scheduleRepository, IEventBus eventBus)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _eventBus = eventBus;
    }

    public async Task<EmployeeDto> Handle(AddScheduleToEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получен запрос на добавление расписания сотруднику: {EmployeeId}", request.EmployeeId);

        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

        if (employee == null)
        {
            _logger.LogWarning("Сотрудник с ID {EmployeeId} не найден", request.EmployeeId);
            throw new NotFoundException(nameof(Employee), request.EmployeeId);
        }

        _logger.LogInformation("Сотрудник найден: {EmployeeId}", employee.Id);

        var dayOfWeek = Domain.SeedWork.Enumeration.FromDisplayName<CustomDateOfWeek>(request.DayOfWeek);

        if (dayOfWeek == null)
        {
            _logger.LogWarning("Неверный день недели: {DayOfWeek}", request.DayOfWeek);
            throw new NotFoundException("Day of week not found");
        }

        var schedule = Schedule.Create(
            Guid.NewGuid(),
            dayOfWeek,
            request.StartTime,
            request.EndTime
        );

        _logger.LogInformation("Создан новый Schedule: {ScheduleId} [{DayOfWeek}] {StartTime} - {EndTime}", 
            schedule.Id, dayOfWeek.Name, request.StartTime, request.EndTime);
        

        employee.AddSchedule(schedule);
        await _scheduleRepository.CreateAsync(schedule);
        await _scheduleRepository.SaveChangesAsync();
        
        await _eventBus.SendMessageAsync(new ScheduleAddedEmployeeEvent(
            employee.Id,
            schedule.Id,
            schedule.DateOfWeekId,
            schedule.StartTime, 
            schedule.EndTime    
        ), cancellationToken);

        return _mapper.Map<EmployeeDto>(employee);
    }
}
