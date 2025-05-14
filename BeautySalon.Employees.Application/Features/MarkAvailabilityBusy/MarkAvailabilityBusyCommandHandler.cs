using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.MarkAvailabilityBusy;

public class MarkAvailabilityBusyCommandHandler : IRequestHandler<MarkAvailabilityBusyCommand>
{
    private readonly IEmployeeRepository _repository;

    public MarkAvailabilityBusyCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(MarkAvailabilityBusyCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.EmployeeId);

        if (employee == null)
            throw new NotFoundException("Сотрудник не найден.");

        employee.MarkAvailabilityBusy(request.AvailabilityId);

        await _repository.SaveChangesAsync();
    }
}
