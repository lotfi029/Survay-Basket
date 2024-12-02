﻿using FluentValidation;

namespace Survay_Basket.API.Contracts.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest> 
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .Length(3, 100);
    }
}