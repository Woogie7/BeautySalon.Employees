using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Employees;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.UpdateEmployee;

public sealed class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
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

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id); // с Include

        if (employee == null)
            throw new NotFoundException("Сотрудник не найден.");

        employee.UpdateName(request.FirstName, request.LastName);
        employee.UpdateEmail(request.Email);
        employee.UpdatePhone(request.Phone);
        
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

        return _mapper.Map<EmployeeDto>(employee);
    }
}
