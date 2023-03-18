using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(a => a.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Email address is required!")
                .NotNull().WithMessage("Email address is required!")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("A valid email is required!");

            RuleFor(a => a.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Password is required!")
                .NotNull().WithMessage("Password is required!")
                .MinimumLength(8).WithMessage("Password is at least 8 characters!");
        }
    }
}
