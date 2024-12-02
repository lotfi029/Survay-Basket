using FluentValidation;
using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(p => p.CurrentPassword)
            .NotEmpty();

        RuleFor(e => e.NewPassword)
            .NotEmpty()
            .Matches(RegaxPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric, and Uppercase")
            .NotEqual(c => c.CurrentPassword)
            .WithMessage("New Password should not be equal to current Pass!");



    }
}
