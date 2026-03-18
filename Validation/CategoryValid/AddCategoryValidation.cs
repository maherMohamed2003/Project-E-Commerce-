using E_Commerce_Proj.DTOs.CategoryDTOs;
using FluentValidation;

namespace E_Commerce_Proj.Validation.CategoryValid
{
    public class AddCategoryValidation : AbstractValidator<AddCategoryDTO>
    {
        public AddCategoryValidation()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(5).WithMessage("Category name must not exceed 5 characters.");
        }
    }
}
