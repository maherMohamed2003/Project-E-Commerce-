using E_Commerce_Proj.DTOs.User;
using FluentValidation;

namespace E_Commerce_Proj.Validation.Customer
{
    public class LoginValidation : AbstractValidator<LoginDTO>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress()
                .WithMessage("Invalid Email Address");

            RuleFor(x => x.Password)
                .NotNull()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");
        }
    }
}
