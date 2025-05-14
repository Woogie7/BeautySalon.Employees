using MediatR;

namespace BeautySalon.Employees.Application.Features.MarkAvailabilityBusy;

public record MarkAvailabilityBusyCommand(Guid EmployeeId, Guid AvailabilityId) : IRequest;
