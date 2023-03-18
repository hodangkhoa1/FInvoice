using BAL.Models;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace BAL.Validators
{
    public class EditProfileValidator : AbstractValidator<EditProfileViewModel>
    {
        public EditProfileValidator()
        {
            RuleFor(ed => ed.FullName)
                .NotEmpty().WithMessage("Full name is required!")
                .NotNull().WithMessage("Full name is required!");

            RuleFor(ed => ed.Email)
                .NotEmpty().WithMessage("Email address is required!")
                .NotNull().WithMessage("Email address is required!")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("A valid email is required!");

            RuleFor(ed => ed.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required!")
                .NotNull().WithMessage("Date of birth is required!");

            RuleFor(ed => ed.Address)
                .NotEmpty().WithMessage("Address is required!")
                .NotNull().WithMessage("Address is required!");

            RuleFor(ed => ed.Phone)
                .NotEmpty().WithMessage("Phone number is required!")
                .NotNull().WithMessage("Phone number is required!")
                .Must(BeAValidPhone).WithMessage("Invalid phone number!");

            RuleFor(ed => ed.Gender)
                .NotEmpty().WithMessage("Gender is required!")
                .NotNull().WithMessage("Gender is required!")
                .Must(BeAValidGender).WithMessage("Invalid gender!");
        }

        private bool BeAValidPhone(string value)
        {
            return Regex.IsMatch(value, @"^(0\d{9,10})$");
        }

        private bool BeAValidGender(char gender)
        {
            if (gender == 'M' || gender == 'F')
                return true;
            return false;
        }
    }
}
