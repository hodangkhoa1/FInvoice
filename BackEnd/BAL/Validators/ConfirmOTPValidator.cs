using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class ConfirmOTPValidator : AbstractValidator<ConfirmOTPViewModel>
    {
        public ConfirmOTPValidator()
        {
            RuleFor(a => a.OtpCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Otp code is required!")
                .NotNull().WithMessage("Otp code is required!");
        }
    }
}
