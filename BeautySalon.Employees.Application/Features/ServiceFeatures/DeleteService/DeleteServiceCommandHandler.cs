using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeleteServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        await _serviceRepository.DeleteAsync(request.ServiceId);
        await _serviceRepository.SaveChangesAsync();
    }
}
