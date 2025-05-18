using BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;
using BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;
using BeautySalon.Employees.Application.Features.ServiceFeatures.GetAllServices;
using MediatR;

namespace BeautySalon.Employees.Api;

public static class ServiceEndpoints
{
    public static IEndpointRouteBuilder MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/services");

        group.MapGet("/", async (ISender mediator) =>
        {
            var allServices = await mediator.Send(new GetAllServicesQuery());
            return Results.Ok(allServices);
        });
        
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

        group.MapPost("/AddServiceToEmployee", async ([AsParameters]Guid emploeeId, [AsParameters]Guid serviceId, ISender mediator) =>
        {
            await mediator.Send(new AddServiceToEmployeeCommand(emploeeId, serviceId));
            return Results.Ok();
        }); 
        
        return app;
    }
}