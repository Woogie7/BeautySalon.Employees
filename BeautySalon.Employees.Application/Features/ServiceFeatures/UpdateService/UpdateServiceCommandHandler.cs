using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Service;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IEventBus _eventBus;

    public UpdateServiceCommandHandler(IServiceRepository serviceRepository, IEventBus eventBus)
    {
        _serviceRepository = serviceRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service is null)
            throw new NotFoundException("Услуга не найдена.");

        service.Update(request.Name, request.Description, request.Duration, request.Price);

        await _serviceRepository.SaveChangesAsync();
        
        await _eventBus.SendMessageAsync(new ServiceUpdatedEvent
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Duration = service.Duration,
            Price = service.Price
        }, cancellationToken);
    }
}
