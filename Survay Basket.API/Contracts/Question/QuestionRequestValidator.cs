using FluentValidation;

namespace Survay_Basket.API.Contracts.Question;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(e => e.Content)
            .NotEmpty()
            .Length(3, 1000);

        RuleFor(e => e.Answers)
            .Must(x => x.Count > 1)
            .WithMessage("Question should has at least 2 answers.")
            .When(e => e.Answers != null);

        RuleFor(e => e.Answers)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You Can Not duplicated answers for the same question.")
            .When(e => e.Answers != null);
    }
}
