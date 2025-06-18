using BeautySalon.Employees.Application.DTO.Schedule;
using BeautySalon.Employees.Application.Features.ScheduleFeatures.UpdateScheduleForEmployee;
using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Application.Validators;

using FluentValidation;

public class AddScheduleRequestValidator : AbstractValidator<AddScheduleRequest>
{
    public AddScheduleRequestValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotEmpty()
            .Must(day => 
            {
                try
                {
                    Enumeration.FromDisplayName<CustomDateOfWeek>(day);
                    return true;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage("DayOfWeek must be a valid day");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("StartTime must be before EndTime");
    }

    private bool BeAValidDayOfWeek(string dayOfWeek)
    {
        return Enum.TryParse<DayOfWeek>(dayOfWeek, true, out _);
    }
}

public class UpdateScheduleForEmployeeCommandValidator : AbstractValidator<UpdateScheduleForEmployeeCommand>
{
    public UpdateScheduleForEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.ScheduleId).NotEmpty();

        RuleFor(x => x.DayOfWeek)
            .NotEmpty()
            .Must(day => 
            {
                try
                {
                    Enumeration.FromDisplayName<CustomDateOfWeek>(day);
                    return true;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage("DayOfWeek must be a valid day");


        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("StartTime must be before EndTime");
    }
}
