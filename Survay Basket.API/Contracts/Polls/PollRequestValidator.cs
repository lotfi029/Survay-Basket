using FluentValidation;

namespace Survay_Basket.API.Contracts.Polls;

public class LoginRequestValidator : AbstractValidator<PollRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.Description)
            .Length(3, 1500);

        RuleFor(e => e.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Today);

        RuleFor(e => e.EndsAt)
            .NotEmpty();
        //.GreaterThanOrEqualTo(e => e.StartsAt);

        RuleFor(e => e)
            .Must(HasValidDates)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} must be greater than opr equal startDate");

        // Validate that the name is unique
    }
    private bool HasValidDates(PollRequest pollRequest)
    {
        return pollRequest.EndsAt >= pollRequest.StartsAt;
    }
}