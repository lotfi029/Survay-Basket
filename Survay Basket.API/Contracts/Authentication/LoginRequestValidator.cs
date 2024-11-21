using FluentValidation;

namespace Survay_Basket.API.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(e => e.Password)
            .NotEmpty();
    }
}