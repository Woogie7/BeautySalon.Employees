using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Service;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IEventBus _eventBus;

    public DeleteServiceCommandHandler(IServiceRepository serviceRepository, IEventBus eventBus)
    {
        _serviceRepository = serviceRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        await _serviceRepository.DeleteAsync(request.ServiceId);
        await _serviceRepository.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new ServiceDeletedEvent
        {
            Id   = request.ServiceId
        }, cancellationToken);
    }
}
