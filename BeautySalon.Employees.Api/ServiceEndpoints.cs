using BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;
using BeautySalon.Employees.Application.Features.ServiceFeatures.GetAllServices;
using MediatR;

namespace BeautySalon.Employees.Api;

public static class ServiceEndpoints
{
    public static IEndpointRouteBuilder MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/services");

        group.MapPost("/", async (CreateServiceCommand command, ISender mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/services/{id}", id);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateServiceCommand command, ISender mediator) =>
        {
            if (id != command.ServiceId)
                return Results.BadRequest("ID mismatch");

            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            await mediator.Send(new DeleteServiceCommand(id));
            return Results.NoContent();
        });

        group.MapGet("/", async (ISender mediator) =>
        {
            var services = await mediator.Send(new GetAllServicesQuery());
            return Results.Ok(services);
        }); 

        return app;
    }
}