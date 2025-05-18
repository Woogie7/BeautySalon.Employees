using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using BeautySalon.Booking.Persistence;
using BeautySalon.Booking.Application.Interface.DB;
using BeautySalon.Booking.Infrastructure.Services;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;

public class IsEmployeeAvailableAsyncTests
{
    private BookingDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BookingDbContext(options);
    }

    private EmployeeReadService CreateService(BookingDbContext context)
    {
        return new EmployeeReadService(context, null); // null — для _cacheService, если не используешь в тесте
    }

    [Fact]
    public async Task Should_ReturnTrue_When_EmployeeAvailable()
    {
        var context = CreateDbContext();

        var employeeId = Guid.NewGuid();
        var start = DateTime.Today.AddHours(10);
        var duration = TimeSpan.FromHours(1);

        // Добавим расписание
        context.Schedules.Add(new Schedule
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            DayOfWeek = start.DayOfWeek,
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(18)
        });

        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.IsEmployeeAvailableAsync(employeeId, start, duration);

        Assert.True(result);
    }

    [Fact]
    public async Task Should_ReturnFalse_When_NoSchedule()
    {
        var context = CreateDbContext();
        var employeeId = Guid.NewGuid();

        var service = CreateService(context);
        var result = await service.IsEmployeeAvailableAsync(employeeId, DateTime.Today.AddHours(10), TimeSpan.FromHours(1));

        Assert.False(result);
    }

    [Fact]
    public async Task Should_ReturnFalse_When_OverlappingBookingExists()
    {
        var context = CreateDbContext();

        var employeeId = Guid.NewGuid();
        var start = DateTime.Today.AddHours(10);
        var duration = TimeSpan.FromHours(1);

        context.Schedules.Add(new Schedule
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            DayOfWeek = start.DayOfWeek,
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(18)
        });

        context.Books.Add(new Book
        {
            Id = Guid.NewGuid(),
            EmployeeId = EmployeeId.Create(employeeId),
            ClientId = ClientId.Create(Guid.NewGuid()),
            ServiceId = ServiceId.Create(Guid.NewGuid()),
            Time = new BookingTime(start.AddMinutes(-15), duration)
        });

        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.IsEmployeeAvailableAsync(employeeId, start, duration);

        Assert.False(result);
    }

    [Fact]
    public async Task Should_ReturnFalse_When_OverlappingAvailabilityExists()
    {
        var context = CreateDbContext();

        var employeeId = Guid.NewGuid();
        var start = DateTime.Today.AddHours(10);
        var duration = TimeSpan.FromHours(1);

        context.Schedules.Add(new Schedule
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            DayOfWeek = start.DayOfWeek,
            StartTime = TimeSpan.FromHours(9),
            EndTime = TimeSpan.FromHours(18)
        });

        context.Availabilities.Add(new EmployeeAvailability
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            StartTime = start.AddMinutes(-30),
            EndTime = start.AddMinutes(30)
        });

        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.IsEmployeeAvailableAsync(employeeId, start, duration);

        Assert.False(result);
    }
}
