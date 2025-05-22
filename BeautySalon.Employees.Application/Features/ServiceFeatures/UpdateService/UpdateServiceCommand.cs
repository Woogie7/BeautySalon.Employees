using MediatR;
using System;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    string Description,
    TimeSpan Duration,
    decimal Price
) : IRequest;