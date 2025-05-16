using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Repository;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee
{
    internal class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper, IServiceRepository serviceRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request);
            
            foreach (var serviceId in request.ServiceIds)
            {
                if (!await _serviceRepository.ExistsAsync(serviceId))
                    throw new NotFoundException($"Service with ID {serviceId} not found");
                employee.AddSkill(serviceId);
            }
            
            await _employeeRepository.CreateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
