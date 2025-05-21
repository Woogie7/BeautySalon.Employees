using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Infrastructure.Rabbitmq.Consumer;

public class BookingEventsConsumer : IConsumer<BookingSlotReservedEvent>
{
    private readonly EmployeeDBContext _context;
    private readonly ILogger<BookingEventsConsumer> _logger;
    private readonly IEventBus _eventBus;

    public BookingEventsConsumer(EmployeeDBContext context, ILogger<BookingEventsConsumer> logger, IEventBus eventBus)
    {
        _context = context;
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task Consume(ConsumeContext<BookingSlotReservedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Получено событие бронирования: BookingId = {BookingId}", message.BookingId);

        var employee = await _context.Employees
            .Include(e => e.Availabilities)
            .FirstOrDefaultAsync(e => e.Id == message.EmployeeId);

        if (employee == null)
        {
            _logger.LogWarning("Сотрудник с ID {EmployeeId} не найден", message.EmployeeId);
            return;
        }

        var availability = Availability.Create(
            message.EmployeeId,
            message.StartTime,
            message.StartTime.Add(message.Duration)
        );

        try
        {
            employee.AddAvailability(availability);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Ошибка при добавлении недоступности: {Message}", ex.Message);
            throw;
        }

        await _context.Availabilities.AddAsync(availability);
        await _context.SaveChangesAsync();

        await _eventBus.SendMessageAsync(new AvailabilityCreatedEvent
        {
            EmployeeId = availability.EmployeeId,
            StartTime = availability.StartTime,
            EndTime = availability.EndTime,
            AvailabilityId = availability.Id
        }, context.CancellationToken);

        _logger.LogInformation("Занятость сотрудника {EmployeeId} обновлена", message.EmployeeId);
    }

}
