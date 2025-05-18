using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Schedule;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ScheduleFeatures.RemoveScheduleFromEmployee;

public class RemoveScheduleFromEmployeeHandler : IRequestHandler<RemoveScheduleFromEmployeeCommand, EmployeeDto>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public RemoveScheduleFromEmployeeHandler(IScheduleRepository scheduleRepository, IEmployeeRepository employeeRepository, IEventBus eventBus, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _eventBus = eventBus;
        _mapper = mapper;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<EmployeeDto> Handle(RemoveScheduleFromEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var schedule = await _scheduleRepository.GetByIdAsync(request.ScheduleId);
        if (schedule == null)
            throw new NotFoundException(nameof(Schedule), request.ScheduleId);

        employee.RemoveSchedule(schedule.Id);
        await _scheduleRepository.DeleteAsync(schedule);
        await _scheduleRepository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new ScheduleRemovedEmployeeEvent(
            employee.Id,
            schedule.Id
        ), cancellationToken);
        
        return _mapper.Map<EmployeeDto>(employee);
    }

}