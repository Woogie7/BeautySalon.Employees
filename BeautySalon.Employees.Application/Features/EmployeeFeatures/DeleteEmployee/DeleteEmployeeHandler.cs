using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Employees;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.DeleteEmployee;

public sealed class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, EmployeeDto?>
{
    private readonly IEmployeeRepository _repository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public DeleteEmployeeHandler(IEmployeeRepository repository, IEventBus eventBus, IMapper mapper)
    {
        _repository = repository;
        _eventBus = eventBus;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdWithSchedulesAsync(request.Id);
        if (employee == null)
            throw new NotFoundException("Сотрудник не найден");
        
        employee.Deactivate();
        
        employee.MarkAllSchedulesUnavailable();

        await _repository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new EmployeeDeletedEvent
        {
            Id = employee.Id
        }, cancellationToken);
        
        return _mapper.Map<EmployeeDto>(employee);
    }
}