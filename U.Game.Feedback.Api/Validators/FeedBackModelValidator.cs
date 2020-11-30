using FluentValidation;
using U.Game.Feedback.Api.Models;

namespace U.Game.Feedback.Api.Validators
{
    public class FeedBackModelValidator : AbstractValidator<FeedbackModel>
    {
        public FeedBackModelValidator()
        {
            RuleFor(r => r.Rating)
                .Must(m => m >= 1 && m <= 5)
                .WithMessage("The rating should be between 1 and 5");

            RuleFor(r => r.Comments)
                .MaximumLength(512);
        }
    }
}
