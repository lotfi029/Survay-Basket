using FluentValidation;

namespace Survay_Basket.API.Contracts.Authentication;

public class ResendConfirmationEmailValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty();
    }
}
