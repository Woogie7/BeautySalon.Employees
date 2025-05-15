using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeesByService;

public class GetEmployeesByServiceQueryHandler : IRequestHandler<GetEmployeesByServiceQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeesByServiceQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesByServiceQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetByServiceIdAsync(request.ServiceId);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}