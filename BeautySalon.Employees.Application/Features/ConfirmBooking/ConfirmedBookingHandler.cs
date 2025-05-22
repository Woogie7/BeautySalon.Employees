using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Contracts;
using BeautySalon.Employees.Application.Exceptions;
using BeautySalon.Employees.Domain;
using MediatR;

namespace BeautySalon.Employees.Application.Features.ConfirmBooking
{
    public class ConfirmBooked : IRequest
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
    }

    internal class ConfirmedBookingHandler : IRequestHandler<ConfirmBooked>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IEventBus _eventBus;

        public ConfirmedBookingHandler(IEventBus eventBus, IEmployeeRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public async Task Handle(ConfirmBooked request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new NotFoundException("Employee not found");

            var endTime = request.StartTime + request.Duration;

            if (request.Status == "Confirmed")
            {
                employee.BookTime(request.StartTime, endTime);
            }
            else if (request.Status == "Canceled")
            {
                employee.CancelBooking(request.StartTime, endTime);
            }
//
            await _repository.SaveChangesAsync();

            await _eventBus.SendMessageAsync(new BookingStatusChangedEvent
            {
                BookingId = request.Id,
                Status = request.Status,
                StartTime = request.StartTime,
                Duration = request.Duration
            }, cancellationToken);
        }
    }
}
