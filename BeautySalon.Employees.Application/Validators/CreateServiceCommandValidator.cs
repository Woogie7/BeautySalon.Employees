using BeautySalon.Employees.Application.Features.ServiceFeatures.CreateService;

namespace BeautySalon.Employees.Application.Validators;

using FluentValidation;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название услуги обязательно.")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero).WithMessage("Длительность должна быть положительной.")
            .LessThanOrEqualTo(TimeSpan.FromHours(3)).WithMessage("Длительность не может превышать 3 часа.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Цена должна быть неотрицательной.");
    }
}


