using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateServiceCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Guid> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = Service.Create(request.Name, request.Description, request.Duration, request.Price);

        await _employeeRepository.CreateServiceAsync(service);
        await _employeeRepository.SaveChangesAsync();

        return service.Id;
    }
}
