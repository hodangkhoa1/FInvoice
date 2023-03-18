using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpViewModel>
    {
        public SignUpValidator()
        {
            RuleFor(a => a.FullName)
                .NotEmpty().WithMessage("Full name is required!")
                .NotNull().WithMessage("Full name is required!");

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email address is required!")
                .NotNull().WithMessage("Email address is required!")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("A valid email is required!");

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required!")
                .NotNull().WithMessage("Password is required!")
                .Length(6, 24).WithMessage("Password must be between {MinLength}..{MaxLength} characters!");

            RuleFor(a => a.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required!")
                .NotNull().WithMessage("Confirm password is required!")
                .Matches(a => a.Password).WithMessage("Confirm Password does not match!");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
        }
    }
}
