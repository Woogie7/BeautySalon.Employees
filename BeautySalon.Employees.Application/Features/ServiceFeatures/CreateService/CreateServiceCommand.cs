using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;

public record CreateServiceCommand(
    string Name,
    string Description,
    TimeSpan Duration,
    decimal Price
) : IRequest<Guid>;