using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;

public record CreateServiceCommand(string Name, string Description, decimal Price, TimeSpan Duration) : IRequest<Guid>;
