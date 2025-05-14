using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Employees.Api;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.Features.AddScheduleToEmployee;
using BeautySalon.Employees.Application.Features.ConfirmBooking;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.GetAllEmployees;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.CheckEmployeeAvailability;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.RemoveScheduleFromEmployee;
using BeautySalon.Employees.Infrastructure;
using BeautySalon.Employees.Persistence;
using MassTransit;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    //busConfing.AddConsumer<>();

    busConfing.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri("amqp://rabbitmq:5672"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("EmployeesChache");
    options.InstanceName = "Employees";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.MigrateDbAsync();

app.MapGet("/employees", async (ISender _sender) =>
{
    var result = await _sender.Send(new GetEmployeesQuery());

    if (result != null && result.Any())
        return Results.Ok(result);

    return Results.NotFound();
});

app.MapPost("/employees", async (CreateEmployeeCommand command, ISender mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/employees/{result.Id}", result);
});

app.MapPost("/employees/{employeeId}/schedules", async (Guid employeeId, AddScheduleToEmployeeCommand command, ISender mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Ok(result);
});

app.MapDelete("/employees/{employeeId}/schedules/{scheduleId}", async (Guid employeeId, Guid scheduleId, ISender mediator) =>
{
    var command = new RemoveScheduleFromEmployeeCommand(employeeId, scheduleId);

    var result = await mediator.Send(command);
    return Results.Ok(result);
});

app.MapGet("/employees/{employeeId}/availability", async (
    Guid employeeId,
    CheckEmployeeAvailabilityCommand command,
    ISender mediator) =>
{
    var isAvailable = await mediator.Send(command);
    return Results.Ok(new { Available = isAvailable });
});

app.MapPost("/confirmed", async (ConfirmBooked reqest, ISender _sender) =>
{
    await _sender.Send(reqest);

    return Results.Ok();
});

app.UseHttpsRedirection();

app.Run();

