using FluentValidation;

namespace Survay_Basket.API.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>  
{
    public VoteRequestValidator()
    {
        RuleFor(e => e.Answers)
            .NotEmpty();

        RuleFor(e => e.Answers)
            .Must(e => e.Distinct().Count() == e.Count())
            .WithMessage("No More One Vote To Same Question.")
            .When(x => x.Answers != null);

        RuleForEach(e => e.Answers)
            .SetInheritanceValidator(e => e.Add(new VoteAnswerRequestValidator()));
    }
}
