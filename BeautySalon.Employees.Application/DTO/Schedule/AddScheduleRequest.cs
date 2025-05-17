using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;

namespace BeautySalon.Employees.Application.DTO.Schedule;

public record AddScheduleRequest(
    string DayOfWeek,  
    TimeSpan StartTime,
    TimeSpan EndTime
);