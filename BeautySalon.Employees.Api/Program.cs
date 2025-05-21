using System.Text;
using BeautySalon.Booking.Infrastructure.Rabbitmq;
using BeautySalon.Employees.Api;
using BeautySalon.Employees.Api.Middleware;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.Features.ConfirmBooking;
using BeautySalon.Employees.Infrastructure;
using BeautySalon.Employees.Infrastructure.Rabbitmq.Consumer;
using BeautySalon.Employees.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()  
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
 
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

Log.Logger.Information("JwtOptions in Employees Service: SecretKey = {SecretKey}, Issuer = {Issuer}, Audience = {Audience}",
    jwtOptions.SecretKey, jwtOptions.Issuer, jwtOptions.Audience);

var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
        
    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
        
    options.AddPolicy("ClientOnly", policy =>
        policy.RequireRole("Client"));
});



builder.Services.AddMassTransit(busConfing =>
{
    busConfing.SetKebabCaseEndpointNameFormatter();

    busConfing.AddConsumer<BookingEventsConsumer>();
    busConfing.AddConsumer<BookingCancelledConsumer>();
    busConfing.AddConsumer<EmployeeCreatedConsumer>();

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

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();



await app.MigrateDbAsync();

app.UseExceptionHandling();


app.MapGet("/debug", (HttpContext ctx) =>
{
    var user = ctx.User;
    return Results.Ok(new
    {
        IsAuthenticated = user.Identity?.IsAuthenticated,
        Name = user.Identity?.Name,
        Claims = user.Claims.Select(c => new { c.Type, c.Value })
    });
}).RequireAuthorization();

app.MapServiceEndpoints();
app.MapScheduleEndpoints();
app.MapEmployeeEndpoints();

app.MapPost("/confirmed", async ([FromBody] ConfirmBooked request, [FromServices] ISender _sender) =>
{
    await _sender.Send(request);

    return Results.Ok();
}).RequireAuthorization("EmployeeOnly");


app.Run();

