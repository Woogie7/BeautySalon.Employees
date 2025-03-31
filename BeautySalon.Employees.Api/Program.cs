using AutoMapper;
using BeautySalon.Employees.Application;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// Принудительное задание кодировки для текстовых ответов
builder.Services.Configure<MvcOptions>(options =>
{
    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.StringOutputFormatter>();
    options.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.StringOutputFormatter
    {
        SupportedEncodings = { Encoding.UTF8 }
    });
});

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

app.MapGet("/employees", () => "Привет, мир! ");

app.MapGet("a", () =>
{
    return Results.Ok("Привет");
});


app.UseHttpsRedirection();

app.Run();

