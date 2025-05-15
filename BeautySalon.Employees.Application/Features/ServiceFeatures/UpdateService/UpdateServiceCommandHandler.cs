using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public UpdateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service is null)
            throw new NotFoundException("Услуга не найдена.");

        service.Update(request.Name, request.Description, request.Duration, request.Price);

        await _serviceRepository.SaveChangesAsync();
    }
}
