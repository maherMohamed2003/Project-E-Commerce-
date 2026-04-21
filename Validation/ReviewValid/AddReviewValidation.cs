using E_Commerce_Proj.DTOs.ReviewDTOs;
using FluentValidation;

namespace E_Commerce_Proj.Validation.ReviewValid
{
    public class AddReviewValidation : AbstractValidator<AddReviewDTO>
    {
        public AddReviewValidation()
        {
            RuleFor(r => r.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

               RuleFor(r => r.ReviewText)
                .MaximumLength(500)
                .WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
