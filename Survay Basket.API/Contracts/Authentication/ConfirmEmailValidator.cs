using FluentValidation;

namespace Survay_Basket.API.Contracts.Authentication;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(e => e.Code)
            .NotEmpty();
        
        RuleFor(e => e.UserId)
            .NotEmpty();
    }
}
