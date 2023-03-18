using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class EditAccountingSoftwareValidator : AbstractValidator<EditAccountingSoftwareViewModel>
    {
        public EditAccountingSoftwareValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Accounting software name is required!")
                .NotNull().WithMessage("Accounting software name is required!");
        }
    }
}
