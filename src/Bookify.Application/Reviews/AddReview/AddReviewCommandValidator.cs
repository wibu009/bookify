using FluentValidation;

namespace Bookify.Application.Reviews.AddReview;

public sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty();
        RuleFor(x => x.Rating)
            .NotEmpty()
            .InclusiveBetween(1, 5);
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500);
    }
}