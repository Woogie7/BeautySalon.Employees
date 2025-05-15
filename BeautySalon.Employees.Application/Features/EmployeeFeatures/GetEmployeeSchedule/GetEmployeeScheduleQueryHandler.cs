using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeeSchedule;

public class GetEmployeeScheduleQueryHandler : IRequestHandler<GetEmployeeScheduleQuery, IEnumerable<ScheduleDto>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeeScheduleQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleDto>> Handle(GetEmployeeScheduleQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdWithSchedulesAsync(request.EmployeeId);
        if (employee == null)
            throw new NotFoundException("Employee not found");

        return _mapper.Map<IEnumerable<ScheduleDto>>(employee.Schedules);
    }
}