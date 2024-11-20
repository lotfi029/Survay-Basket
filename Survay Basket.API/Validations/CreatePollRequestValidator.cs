using FluentValidation;

namespace Survay_Basket.API.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty();
    }
}