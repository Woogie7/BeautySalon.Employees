using AutoMapper;
using BeautySalon.Employees.Application.DTO;
using BeautySalon.Employees.Application.Features.CreateEmployee;
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
        }
    }
}
