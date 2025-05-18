using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Application.Features.AddScheduleToEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.AddScheduleToEmployee;

public class AddScheduleToEmployeeCommandHandler : IRequestHandler<AddScheduleToEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddScheduleToEmployeeCommandHandler> _logger;
    public AddScheduleToEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<AddScheduleToEmployeeCommandHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
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

    var isOverlapping = employee.Schedules.Any(s => s.OverlapsWith(schedule.StartTime, schedule.EndTime));
    _logger.LogInformation("Наложение расписания: {IsOverlapping}", isOverlapping);

    if (isOverlapping)
    {
        _logger.LogWarning("Обнаружено наложение расписаний при добавлении расписания сотруднику {EmployeeId}", employee.Id);
        throw new InvalidOperationException("The new schedule overlaps with an existing one.");
    }

    employee.AddSchedule(schedule);
    _logger.LogInformation("Расписание добавлено в агрегат. Попытка сохранения изменений...");

    try
    {
        await _employeeRepository.SaveChangesAsync();
        _logger.LogInformation("Изменения успешно сохранены в БД");
    }
    catch (DbUpdateConcurrencyException ex)
    {
        _logger.LogError(ex, "Ошибка конкурентности при сохранении расписания сотруднику {EmployeeId}", employee.Id);
        throw;
    }

    return _mapper.Map<EmployeeDto>(employee);
}


}
