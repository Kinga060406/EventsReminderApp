using FluentValidation;
using EventsReminderApp.MVC.Models;

public class EventsValidator : AbstractValidator<Events>
{
    public EventsValidator()
    {
        RuleFor(eventss => eventss.Name)
            .NotNull().WithMessage("Name cannot be null.")
            .MaximumLength(100).WithMessage("Name must be less than 100 characters.");

        RuleFor(eventss => eventss.Date)
            .NotNull().WithMessage("Date cannot be null.");

        RuleFor(eventss => eventss.Time)
            .NotNull().WithMessage("Time cannot be null.");

        RuleFor(eventss => eventss.Description)
            .NotNull().WithMessage("Description cannot be null.");
    }
}
