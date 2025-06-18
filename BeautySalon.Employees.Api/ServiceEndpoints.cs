using BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;
using BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;
using BeautySalon.Employees.Application.Features.ServiceFeatures.GetAllServices;
using FluentValidation;
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
        
        group.MapPost("/", async (
            CreateServiceCommand command, 
            ISender mediator, 
            IValidator<CreateServiceCommand> validator) =>
        {
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var id = await mediator.Send(command);
            return Results.Created($"/api/services/{id}", id);
        }).RequireAuthorization("AdminOnly");

        group.MapPut("/{id:guid}", async (
            Guid id, 
            UpdateServiceCommand command, 
            ISender mediator,
            IValidator<UpdateServiceCommand> validator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await mediator.Send(command);
            return Results.NoContent();
        }).RequireAuthorization("AdminOnly");


        group.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            await mediator.Send(new DeleteServiceCommand(id));
            return Results.NoContent();
        }).RequireAuthorization("AdminOnly");
        
        return app;
    }
}