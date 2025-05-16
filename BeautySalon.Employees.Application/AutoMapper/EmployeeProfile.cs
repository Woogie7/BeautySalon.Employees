using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeautySalon.Employees.Application.Features.EmployeeFeatures.CreateEmployee;
using BeautySalon.Employees.Domain.ValueObjects;

namespace BeautySalon.Employees.Application.AutoMapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {

            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();
            CreateMap<CreateEmployeeCommand, Employee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new FullName(src.FirstName, src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new PhoneNumber(src.Phone)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Schedules, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.Ignore());

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Skills.Select(s => s.Service)))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules));

            CreateMap<Service, ServiceDto>();

            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.DateOfWeekName, opt => opt.MapFrom(src => src.DateOfWeek.Name));
        }
    }
}
