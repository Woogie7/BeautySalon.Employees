using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BeautySalon.Employees.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            service.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            service.AddFluentValidationAutoValidation();
            
            return service;
            
        }
    }
}
