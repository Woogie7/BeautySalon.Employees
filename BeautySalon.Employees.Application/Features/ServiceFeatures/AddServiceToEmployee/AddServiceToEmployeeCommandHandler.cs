using AutoMapper;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts.Employees;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;

public class AddServiceToEmployeeCommandHandler : IRequestHandler<AddServiceToEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeDBContext _context;
    private readonly  IEventBus _eventBus;
    private readonly IMapper _mapper;

    public AddServiceToEmployeeCommandHandler(IEmployeeRepository employeeRepository, EmployeeDBContext context, IEventBus eventBus, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _context = context;
        _eventBus = eventBus;
        _mapper = mapper;
    }
    
    public async Task<EmployeeDto> Handle(AddServiceToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee is null)
            throw new NotFoundException("Сотрудник не найден.");

        var service = await _employeeRepository.GetServiceByIdAsync(request.ServiceId);
        if (service is null)
            throw new NotFoundException("Услуга не найдена.");

        var alreadyExists = await _context.Skills.AnyAsync(s =>
            s.EmployeeId == request.EmployeeId && s.ServiceId == request.ServiceId, cancellationToken);
        if (alreadyExists)
            throw new BadRequestException("Связь уже существует");

        var skill = Skill.Create(employee.Id, service.Id);

        _context.Skills.Add(skill);
        await _context.SaveChangesAsync(cancellationToken);
        
        await _eventBus.SendMessageAsync(new EmployeeUpdatedEvent
        {
            Id = employee.Id,
            Name = employee.Name.First + " " + employee.Name.Last,
            Email = employee.Email.Value,
            Phone = employee.Phone.Value,
            IsActive = employee.IsActive,
            ServiceIds = employee.Skills.Select(s => s.ServiceId).ToList(),
        }, cancellationToken);
        
        
        return _mapper.Map<EmployeeDto>(employee);
    }
}
