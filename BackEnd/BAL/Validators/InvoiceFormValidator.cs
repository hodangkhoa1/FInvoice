using BAL.Models;
using FluentValidation;

namespace BAL.Validators
{
    public class InvoiceFormValidator : AbstractValidator<InvoiceFormViewModel>
    {
        public InvoiceFormValidator()
        {
            RuleFor(f => f.CodeForm)
                .NotEmpty().WithMessage("Code form is required!")
                .NotNull().WithMessage("Code form is required!");

            RuleFor(f => f.NameInvoiceType)
                .NotEmpty().WithMessage("Name invoice type is required!")
                .NotNull().WithMessage("Name invoice type is required!");
        }
    }
}
