using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Employees.Api;
using BeautySalon.Employees.Api.Middleware;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.Features.ConfirmBooking;
using BeautySalon.Employees.Infrastructure;
using BeautySalon.Employees.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()  
    .CreateLogger();

builder.Host.UseSerilog();

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

app.UseExceptionHandling();

app.MapServiceEndpoints();
app.MapScheduleEndpoints();
app.MapEmployeeEndpoints();

app.MapPost("/confirmed", async ([FromBody] ConfirmBooked request, [FromServices] ISender _sender) =>
{
    await _sender.Send(request);

    return Results.Ok();
});

app.UseHttpsRedirection();

app.Run();

