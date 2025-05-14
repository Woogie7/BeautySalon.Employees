using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;

public class AddServiceToEmployeeCommandHandler : IRequestHandler<AddServiceToEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public AddServiceToEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
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

        await _employeeRepository.SaveChangesAsync();
    }
}
