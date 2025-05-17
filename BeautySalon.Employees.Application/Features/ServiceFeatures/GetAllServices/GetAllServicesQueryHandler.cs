using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.GetAllServices;

public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<ServiceDto>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(s => new ServiceDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Duration = s.Duration,
            Price = s.Price
        });
    }
}
