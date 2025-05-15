using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.GetAllServices;

public record GetAllServicesQuery() : IRequest<IEnumerable<ServiceDto>>;