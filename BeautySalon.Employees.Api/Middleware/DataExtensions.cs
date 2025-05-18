using BeautySalon.Employees.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Api;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var DbContext = scope.ServiceProvider.GetRequiredService<EmployeeDBContext>();
        await DbContext.Database.MigrateAsync();
    }
}