using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class EditSupplierValidator : AbstractValidator<EditSupplierViewModel>
    {
        public EditSupplierValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Supplier name is required!")
                .NotNull().WithMessage("Supplier name is required!");
        }
    }
}
