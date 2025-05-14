using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<Employee>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<ScheduleDto> Schedules { get; set; } // Расписание сотрудника

    public UpdateEmployeeCommand(Guid id, string firstName, string lastName, string email, string phone, List<ScheduleDto> schedules)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Schedules = schedules ?? new List<ScheduleDto>();
    }
}