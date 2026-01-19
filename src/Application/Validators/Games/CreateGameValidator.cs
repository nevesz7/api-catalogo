using Application.Dtos.Games;
using Domain.Entities;
using FluentValidation;

namespace Validators.Games
{
    public class CreateGameValidator : AbstractValidator<CreateGameDto>
    {
        public CreateGameValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(g => g.Genre)
                .IsInEnum().WithMessage("Genre is invalid");

            RuleFor(g => g.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be 0 or more");

            RuleFor(g => g.AgeRating)
                .IsInEnum().WithMessage("Age rating is invalid");

            RuleForEach(g => g.Tags)
                .MaximumLength(30).WithMessage("Each tag cannot exceed 30 characters");
        }
    }
}
