using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using MediatR;

namespace BeautySalon.Employees.Application.Features.UpdateEmployee;

public sealed class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _repository;
    private readonly ScheduleMapper _scheduleMapper;

    public UpdateEmployeeHandler(IEmployeeRepository repository, ScheduleMapper scheduleMapper)
    {
        _repository = repository;
        _scheduleMapper = scheduleMapper;
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
                var newSchedule = _scheduleMapper.ToEntity(scheduleDto, dateOfWeek);
                employee.AddSchedule(newSchedule);
            }
        }
        
        await _repository.SaveChangesAsync();

        return employee;
    }
}
public class ScheduleMapper
{
    public Schedule ToEntity(ScheduleDto scheduleDto, CustomDateOfWeek dateOfWeek)
    {
        return new Schedule(
            scheduleDto.Id, 
            dateOfWeek, 
            scheduleDto.StartTime, 
            scheduleDto.EndTime
        );
    }
}
