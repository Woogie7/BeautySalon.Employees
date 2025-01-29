using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/employees", async (CreateEmployeeRequest createEmployeeRequest, ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateEmployeeCommand>(createEmployeeRequest);

    var result = _sender.Send(command);

    return Results.Created();
});

app.UseHttpsRedirection();

app.Run();

