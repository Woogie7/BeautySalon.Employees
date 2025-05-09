using BeautySalon.Booking.Infrastructure.Rabbitmq;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalon.Employees.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddTransient<IEventBus, EventBus>();
        return service;
    }
}