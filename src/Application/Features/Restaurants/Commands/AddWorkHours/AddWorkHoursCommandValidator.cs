namespace Application.Features.Restaurants.Commands.AddWorkHours
{
    // Create Validator for AddWorkHoursCommand

    public class AddWorkHoursCommandValidator : AbstractValidator<AddWorkHoursCommand>
    {
        public AddWorkHoursCommandValidator()
        {
            RuleFor(x => x.RestaurantId)
                .GreaterThan(0);

            RuleFor(x => x.WorkHours)
                .NotEmpty();

            RuleForEach(x => x.WorkHours).ChildRules(work =>
            {
                work.RuleFor(w => w.DayOfWeek)
                    .IsInEnum();

                work.RuleFor(w => w)
                    .Must(w => w.IsClosed || (w.OpeningTime.HasValue && w.ClosingTime.HasValue))
                    .WithMessage("Opening and Closing times are required if the day is not closed.");

                work.RuleFor(w => w)
                    .Must(w => w.IsClosed || w.OpeningTime < w.ClosingTime)
                    .WithMessage("Opening time must be before closing time.");
            });
        }
    }
}
