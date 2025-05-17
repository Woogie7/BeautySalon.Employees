using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Service;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEventBus _eventBus;

    public CreateServiceCommandHandler(IEmployeeRepository employeeRepository, IEventBus eventBus)
    {
        _employeeRepository = employeeRepository;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = Service.Create(request.Name, request.Description, request.Duration, request.Price);

        await _employeeRepository.CreateServiceAsync(service);
        await _employeeRepository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new ServiceCreatedEvent
        {
            Id = service.Id,
            Name = service.Name,
            Duration = service.Duration,
            Description = service.Description,
            Price = service.Price
        }, cancellationToken);
        
        return service.Id;
    }
}
