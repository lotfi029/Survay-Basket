using FluentValidation;
using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(e => e.Password)
            .NotEmpty()
            .Matches(RegaxPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric, and Uppercase");

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .Length(3, 100); 
    }
}
