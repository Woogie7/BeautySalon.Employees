using AutoMapper;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
    string responseText = "lAST �������?";
    // ���������� Results.Text ��� text/plain � ���� ��������� UTF-8
    return Results.Text(responseText, "text/plain", Encoding.UTF8);
});

app.MapGet("a", () =>
{
    return Results.Ok("������");
});


app.UseHttpsRedirection();

app.Run();

