using MediatR;
using System;

public record DeleteServiceCommand(Guid ServiceId) : IRequest;