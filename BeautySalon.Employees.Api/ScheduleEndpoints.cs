using BeautySalon.Employees.Application.Features.AddScheduleToEmployee;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.CheckEmployeeAvailability;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.RemoveScheduleFromEmployee;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.UpdateScheduleForEmployee;
using MediatR;

namespace BeautySalon.Employees.Api;

public static class ScheduleEndpoints
{
    public static IEndpointRouteBuilder MapScheduleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees/{employeeId:guid}/schedules");

        group.MapPost("/", async (Guid employeeId, AddScheduleToEmployeeCommand command, ISender mediator) =>
        {
            if (employeeId != command.EmployeeId)
                return Results.BadRequest("Employee ID mismatch");

            var employee = await mediator.Send(command);
            return Results.Ok(employee);
        });

        group.MapPut("/{scheduleId:guid}", async (Guid employeeId, Guid scheduleId, UpdateScheduleForEmployeeCommand command, ISender mediator) =>
        {
            if (employeeId != command.EmployeeId || scheduleId != command.ScheduleId)
                return Results.BadRequest("ID mismatch");

            var employee = await mediator.Send(command);
            return Results.Ok(employee);
        });

        group.MapDelete("/{scheduleId:guid}", async (Guid employeeId, Guid scheduleId, ISender mediator) =>
        {
            var employee = await mediator.Send(new RemoveScheduleFromEmployeeCommand(employeeId, scheduleId));

            return Results.Ok(employee);
        });

        group.MapGet("/check-availability", async (
            Guid employeeId,
            [AsParameters] CheckAvailabilityRequest request,
            ISender mediator) =>
        {
            var command = new CheckEmployeeAvailabilityCommand
            {
                EmployeeId = employeeId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            var available = await mediator.Send(command);
            return Results.Ok(available);
        });

        return app;
    }

    public record CheckAvailabilityRequest(
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    );
}