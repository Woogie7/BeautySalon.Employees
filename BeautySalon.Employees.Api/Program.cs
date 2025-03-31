using AutoMapper;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/employees", async (CreateEmployeeRequest createEmployeeRequest, [FromServices]ISender _sender, IMapper _mapper) =>
{
    var command = _mapper.Map<CreateEmployeeCommand>(createEmployeeRequest);

    var result = _sender.Send(command);

    return Results.Created();
});

app.MapGet("/employees", () =>
{
    return Results.Text("Привет, мир!", "text/plain; charset=utf-8");
});


app.MapGet("a", () =>
{
    return Results.Ok("Привет");
});


app.UseHttpsRedirection();

app.Run();

