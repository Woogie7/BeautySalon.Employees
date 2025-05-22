using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;

public class AddServiceToEmployeeCommandHandler : IRequestHandler<AddServiceToEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeDBContext _context;

    public AddServiceToEmployeeCommandHandler(IEmployeeRepository employeeRepository, EmployeeDBContext context)
    {
        _employeeRepository = employeeRepository;
        _context = context;
    }

    public async Task Handle(AddServiceToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee is null)
            throw new NotFoundException("Сотрудник не найден.");

        var service = await _employeeRepository.GetServiceByIdAsync(request.ServiceId);
        if (service is null)
            throw new NotFoundException("Услуга не найдена.");

        employee.AddSkill(service.Id);

        var skill = Skill.Create(employee.Id, service.Id);
        _context.Skills.Add(skill); 
        await _context.SaveChangesAsync();
        
        await _employeeRepository.SaveChangesAsync();
    }
}
