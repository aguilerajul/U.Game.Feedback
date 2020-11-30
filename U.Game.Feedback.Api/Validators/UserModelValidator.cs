using FluentValidation;
using U.Game.Feedback.Api.Models;

namespace U.Game.Feedback.Api.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("The User Name field cannot be empty.")
                .MaximumLength(150);

            RuleFor(r => r.NickName)
                .NotEmpty()
                .WithMessage("The Nickname field cannot be empty.")
                .MaximumLength(150);

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("The Email field cannot be empty.")
                .EmailAddress()
                .WithMessage("Please provide a valid Email")
                .MaximumLength(250);
        }
    }
}
