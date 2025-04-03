using AutoMapper;
using BeautySalon.Employees.Api;
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

app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Type"] = "text/plain; charset=utf-8";
    await next();
});


app.MapPost("/api/tasks", async (TaskRequest taskRequest) =>
{
    if (taskRequest == null)
    {
        return Results.BadRequest("Request body is required.");
    }

    if (taskRequest.Duration <= 0)
    {
        return Results.BadRequest("Duration must be greater than zero.");
    }

    // Создаем объект задачи для ответа
    var newTask = new
    {
        TaskId = taskRequest.TaskId,
        ProjectId = taskRequest.ProjectId,
        StartTime = taskRequest.StartTime,
        Duration = taskRequest.Duration,
        Status = taskRequest.Status
    };

    // URI созданного ресурса
    var resourceUri = $"/api/tasks/{taskRequest.ProjectId}";

    // Возвращаем HTTP 201 Created с URI и данными задачи
    return Results.Created(resourceUri, newTask);
});

app.MapGet("/api/tasks", (int employeeId) =>
{
    var hardcodedJson = new
    {
        taskid = "789",
        projectId = "456",
        startTime = "2024-12-07T09:00:00",
        duration = 120,
        status = "In Progress"
    };

    return Results.Ok(hardcodedJson);
});

app.MapDelete("/api/tasks", (int idTask) =>
{
    return Results.NoContent();
});

app.MapGet("/employees", () =>
{
    return Results.Text("Всем привет!", "text/plain; charset=utf-8");
});


app.MapGet("a", () =>
{
    return Results.Ok("Привет");
});


app.UseHttpsRedirection();

app.Run();

