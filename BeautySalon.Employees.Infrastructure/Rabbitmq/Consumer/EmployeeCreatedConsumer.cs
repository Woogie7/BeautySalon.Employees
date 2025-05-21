using BeautySalon.Contracts.Employees;
using BeautySalon.Employees.Domain.ValueObjects;
using BeautySalon.Employees.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Infrastructure.Rabbitmq.Consumer;

public class EmployeeCreatedConsumer : IConsumer<EmployeeCreatedEvent>
{
    private readonly EmployeeDBContext _context;
    private readonly ILogger<EmployeeCreatedConsumer> _logger;

    public EmployeeCreatedConsumer(EmployeeDBContext context, ILogger<EmployeeCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Получено событие создания сотрудника: UserId = {UserId}, Email = {Email}", message.Id, message.Email);

        var existing = await _context.Employees.AnyAsync(e => e.Id == message.Id);
        if (existing)
        {
            _logger.LogWarning("Сотрудник с UserId {UserId} уже существует", message.Id);
            return;
        }

        var employee = Domain.Employee.Create(
            message.Id,
            new FullName(message.Name, ""),
            new Email(message.Email),
            new PhoneNumber(message.Phone)
            );

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Сотрудник {FullName} успешно создан", employee.Name);
    }
}