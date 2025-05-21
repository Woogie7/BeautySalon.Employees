using BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.DeleteEmployee;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.GetAllEmployees;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeeById;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeesByService;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.GetEmployeeSchedule;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.UpdateEmployee;
using BeautySalon.Employees.Application.Features.ServiceFeatures.AddServiceToEmployee;
using BeautySalon.Employees.Application.Features.UpdateEmployee;
using MediatR;

namespace BeautySalon.Employees.Api;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var employees = app.MapGroup("/employees").RequireAuthorization();


        employees.MapGet("/", async (ISender mediator) =>
        {
            var allEmployees = await mediator.Send(new GetAllEmployeesQuery());
            return Results.Ok(allEmployees);
        });
        
        // employees.MapPost("/", async (CreateEmployeeCommand command, IMediator mediator) =>
        // {
        //     var employee = await mediator.Send(command);
        //     return Results.Created($"/employees/{employee.Id}", employee);
        // })
        // .RequireAuthorization("AdminOnly");
        
        employees.MapPut("/{id:guid}", async (Guid id, UpdateEmployeeCommand command, IMediator mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("Id в URL и теле запроса не совпадают");

            var updatedEmployee = await mediator.Send(command);
            return Results.Ok(updatedEmployee);
        })
        .RequireAuthorization("AdminOnly");
        
        employees.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var deletedEmployee = await mediator.Send(new DeleteEmployeeCommand(id));
            if (deletedEmployee == null)
                return Results.NotFound();

            return Results.Ok(deletedEmployee);
        })
        .RequireAuthorization("AdminOnly");
        
        employees.MapGet("/{id:guid}/schedule", async (Guid id, IMediator mediator) =>
        {
            var schedule = await mediator.Send(new GetEmployeeScheduleQuery(id));
            return schedule is not null ? Results.Ok(schedule) : Results.NotFound();
        });
        
        employees.MapGet("/by-service/{serviceId:guid}", async (Guid serviceId, IMediator mediator) =>
        {
            var employeesByService = await mediator.Send(new GetEmployeesByServiceQuery(serviceId));
            return Results.Ok(employeesByService);
        });
        
        employees.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var employee = await mediator.Send(new GetEmployeeByIdQuery(id));
            return employee is not null ? Results.Ok(employee) : Results.NotFound();
        });
        
        employees.MapPost("/{employeeId:guid}/services/{serviceId:guid}", async (Guid employeeId, Guid serviceId, ISender mediator) =>
        {
            await mediator.Send(new AddServiceToEmployeeCommand(employeeId, serviceId));
            return Results.Ok();
        }).RequireAuthorization("EmployeeOnly");
        
        
    }
}