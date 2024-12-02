using FluentValidation;
using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Contracts.Authentication;

public class ResetPaswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPaswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegaxPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric, and Uppercase"); ;

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
