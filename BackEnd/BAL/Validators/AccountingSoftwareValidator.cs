using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class AccountingSoftwareValidator : AbstractValidator<AccountingSoftwareViewModel>
    {
        public AccountingSoftwareValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Accounting software name is required!")
                .NotNull().WithMessage("Accounting software name is required!");
        }
    }
}
