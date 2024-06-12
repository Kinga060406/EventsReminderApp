using FluentValidation;
using EventsReminderApp.MVC.Models;
using System;

public class EventsValidator : AbstractValidator<Events>
{
    public EventsValidator()
    {
        RuleFor(eventss => eventss.Name)
            .NotNull().WithMessage("Name cannot be null.")
            .MaximumLength(100).WithMessage("Name must be less than 100 characters.");

        RuleFor(eventss => eventss.Date)
            .NotNull().WithMessage("Date cannot be null.")
            .Must(BeAValidDate).WithMessage("Date must be today or in the future.");

        RuleFor(eventss => eventss.Time)
            .NotNull().WithMessage("Time cannot be null.")
            .Must((events, time) => IsValidTime(events.Date, time))
            .WithMessage("The time for today must be in the future.");

        RuleFor(eventss => eventss.Description)
            .NotNull().WithMessage("Description cannot be null.");
    }

    private bool BeAValidDate(DateOnly date)
    {
        return date >= DateOnly.FromDateTime(DateTime.Now);
    }

    private bool IsValidTime(DateOnly date, TimeOnly time)
    {
        var now = DateTime.Now;
        if (date == DateOnly.FromDateTime(now))
        {
            return time.ToTimeSpan() > now.TimeOfDay;
        }
        return true; // If the date is in the future, the time is always valid
    }
}
