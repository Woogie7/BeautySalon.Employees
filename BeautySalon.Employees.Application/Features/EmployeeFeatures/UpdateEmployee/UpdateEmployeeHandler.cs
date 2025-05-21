using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Employees;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.UpdateEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;

namespace BeautySalon.Employees.Application.Features.UpdateEmployee;

public sealed class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _repository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public UpdateEmployeeHandler(IEmployeeRepository repository, IEventBus eventBus, IMapper mapper)
    {
        _repository = repository;
        _eventBus = eventBus;
        _mapper = mapper;
    }

    public async Task<Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id);

        if (employee == null)
        {
            throw new NotFoundException("Сотрудник не найден.");
        }
        
        foreach (var scheduleDto in request.Schedules)
        {
            var dateOfWeek = employee.Schedules
                .FirstOrDefault(s => s.DateOfWeekId == scheduleDto.DateOfWeekId)?.DateOfWeek;

            if (dateOfWeek == null)
            {
                throw new NotFoundException("Дата недели не найдена.");
            }

            var existingSchedule = employee.Schedules.FirstOrDefault(s => s.Id == scheduleDto.Id);

            if (existingSchedule != null)
            {
                existingSchedule.UpdateSchedule(dateOfWeek, scheduleDto.StartTime, scheduleDto.EndTime);
            }
            else
            {
                var newSchedule = _mapper.Map<Schedule>(scheduleDto);
                employee.AddSchedule(newSchedule);
            }
        }
        
        await _repository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new EmployeeUpdatedEvent
        {
            Id = employee.Id,
            Name = employee.Name.First + " " + employee.Name.Last,
            Email = employee.Email.Value,
            Phone = employee.Phone.Value,
            IsActive = employee.IsActive,
            ServiceIds = employee.Skills.Select(s => s.ServiceId).ToList(),
        }, cancellationToken);
        
        return employee;
    }
}
