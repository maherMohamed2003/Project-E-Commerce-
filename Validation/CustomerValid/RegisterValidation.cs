using E_Commerce_Proj.DTOs.User;
using FluentValidation;

namespace E_Commerce_Proj.Validation.Customer
{
    public class RegisterValidation : AbstractValidator<RegisterDTO>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .WithMessage("Invalid Email Address!");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long!");

            RuleFor(x => x.FName)
                .Must(x => x.Length > 2 && x.Length <= 20)
                .NotNull()
                .WithMessage("First Name must be between 3 and 20 characters long!");
            
            RuleFor(x => x.LName)
                .Must(x => x.Length > 2 && x.Length <= 20)
                .NotNull()
                .WithMessage("Last Name must be between 3 and 20 characters long!");
        }
    }
}
