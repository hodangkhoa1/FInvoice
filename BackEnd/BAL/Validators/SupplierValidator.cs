using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class SupplierValidator : AbstractValidator<SupplierViewModel>
    {
        public SupplierValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Supplier name is required!")
                .NotNull().WithMessage("Supplier name is required!");
        }
    }
}
