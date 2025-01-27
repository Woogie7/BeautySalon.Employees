using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Persistence;

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

app.MapGet("/employees", async (IEmployeeRepository _employeeRepository) =>
{ 
    return Results.Ok(_employeeRepository.GetAllAsync());
});

app.UseHttpsRedirection();

app.Run();

