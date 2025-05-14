using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.DeleteEmployee;

public sealed class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, Employee?>
{
    private readonly IEmployeeRepository _repository;

    public DeleteEmployeeHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Employee?> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdWithScheduleAsync(request.Id);
        if (employee == null)
            throw new NotFoundException("Сотрудник не найден");
        employee.Deactivate();
        
        employee.MarkAllSchedulesUnavailable();

        await _repository.SaveChangesAsync();
        return employee;
    }
}