using E_Commerce_Proj.DTOs.CartDTOs;
using FluentValidation;

namespace E_Commerce_Proj.Validation.CartValid
{
    public class AddCartItemValidation : AbstractValidator<AddCartItemDTO>
    {
        public AddCartItemValidation()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
