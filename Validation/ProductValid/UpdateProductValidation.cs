using E_Commerce_Proj.DTOs.ProductDTOs;
using FluentValidation;

namespace E_Commerce_Proj.Validation.ProductValid
{
    public class UpdateProductValidation :AbstractValidator<UpdateProductDTO>
    {

        public UpdateProductValidation()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(50).WithMessage("Product name must not exceed 5 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Product quantity must be greater than 0.");

        }
    }
}
