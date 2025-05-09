using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
using BeautySalon.Employees.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Employees.Application.AutoMapper
{
    internal class EmployeeProfile : Profile
    {
        public EmployeeProfile() {

            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();

            CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            //.ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => MapStatusToId(src.status)))asdasdasdasasdasasdasdsadas
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skill.Select(s => new Skill { Name = s }).ToList()))
            .ForMember(dest => dest.Schedules, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());
        }
    }
}
