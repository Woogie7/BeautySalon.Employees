using MediatR;
using System;

public record UpdateServiceCommand(
    Guid ServiceId,
    string Name,
    string Description,
    TimeSpan Duration,
    decimal Price
) : IRequest;