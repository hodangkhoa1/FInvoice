using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(r => r.NewPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("New password is required!")
                .NotNull().WithMessage("New password is required!")
                .Length(6, 24).WithMessage("New password must be between {MinLength}..{MaxLength} characters!");

            RuleFor(r => r.ComrfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Comrfirm password is required!")
                .NotNull().WithMessage("Comrfirm password is required!")
                .Matches(r => r.NewPassword).WithMessage("New Password and Confirm Password do not match!");
        }
    }
}
