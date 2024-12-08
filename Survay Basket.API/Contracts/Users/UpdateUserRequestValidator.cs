using FluentValidation;

namespace Survay_Basket.API.Contracts.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Roles)
            .NotEmpty()
            .Must(e => e.Distinct().Count() == e.Count)
            .WithMessage("You Can't insert duplicated roles.")
            .When(e => e.Roles is not null);
    }
}
