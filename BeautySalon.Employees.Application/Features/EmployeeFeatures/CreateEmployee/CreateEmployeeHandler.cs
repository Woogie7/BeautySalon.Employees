using AutoMapper;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee
{
    internal class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request);
            
            foreach (var serviceId in request.ServiceIds)
            {
                employee.AddSkill(serviceId);
            }
            
            await _employeeRepository.CreateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return employee;
        }
    }
}
