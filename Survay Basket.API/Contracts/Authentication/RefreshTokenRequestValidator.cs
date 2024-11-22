using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Survay_Basket.API.Contracts.Authentication;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.Token).NotEmpty();
        RuleFor(e => e.RefreshToken).NotEmpty();
    }
}
