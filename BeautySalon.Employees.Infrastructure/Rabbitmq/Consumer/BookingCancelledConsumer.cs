using BeautySalon.Contracts;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence.Context;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BeautySalon.Employees.Infrastructure.Rabbitmq.Consumer;

public sealed class BookingCancelledConsumer : IConsumer<BookingCancelledEvent>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeDBContext _context;
    private readonly ILogger<BookingCancelledConsumer> _logger;

    public BookingCancelledConsumer(IEmployeeRepository employeeRepository, ILogger<BookingCancelledConsumer> logger, EmployeeDBContext context)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _context = context;
    }

    public async Task Consume(ConsumeContext<BookingCancelledEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Processing BookingCancelledEvent for Booking ID: {BookingId}", message.BookingId);

        var employee = await _employeeRepository.GetByIdAsync(message.EmployeeId);
        if (employee == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found", message.EmployeeId);
            return;
        }

        var availabilityToRemove = employee.RemoveAvailability(message.StartTime, message.Duration);
        if (availabilityToRemove == null)
        {
            _logger.LogWarning("Availability slot not found for removal: EmployeeId = {EmployeeId}, Start = {Start}, Duration = {Duration}",
                message.EmployeeId, message.StartTime, message.Duration);
            return;
        }
        
        _context.Availabilities.Remove(availabilityToRemove);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Availability slot released for Employee ID: {EmployeeId}, Start: {Start}, Duration: {Duration}",
            message.EmployeeId, message.StartTime, message.Duration);
    }

}
