using Application.Dtos.Games;
using Domain.Entities;
using FluentValidation;
using System.Collections.Generic;

namespace Application.Validators.Games
{
    public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
    {
        public UpdateGameValidator()
        {
            When(g => !string.IsNullOrWhiteSpace(g.Name), () =>
            {
                RuleFor(g => g.Name!)
                    .MaximumLength(100)
                    .WithMessage("Name cannot exceed 100 characters");
            });

            When(g => g.Genre.HasValue, () =>
            {
                RuleFor(g => g.Genre.GetValueOrDefault())
                    .IsInEnum()
                    .WithMessage("Genre is invalid");
            });

            When(g => g.Price.HasValue, () =>
            {
                RuleFor(g => g.Price.GetValueOrDefault())
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Price must be 0 or more");
            });

            When(g => g.AgeRating.HasValue, () =>
            {
                RuleFor(g => g.AgeRating.GetValueOrDefault())
                    .IsInEnum()
                    .WithMessage("Age rating is invalid");
            });

            RuleForEach(g => g.Tags ?? new List<string>())
                .MaximumLength(30)
                .WithMessage("Each tag cannot exceed 30 characters");
        }
    }
}
