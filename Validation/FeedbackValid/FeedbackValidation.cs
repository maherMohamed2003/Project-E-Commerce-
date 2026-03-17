using E_Commerce_Proj.DTOs.Feedback;
using FluentValidation;

namespace E_Commerce_Proj.Validation.Feedback
{
    public class FeedbackValidation : AbstractValidator<AddFeedBackDTO>
    {
        public FeedbackValidation()
        {
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(100).WithMessage("Comment cannot exceed 100 characters.");
        }
    }
}
