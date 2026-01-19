using Application.Dtos.Games;
using FluentValidation;

namespace Validators.Games
{
    public class GameSearchValidator : AbstractValidator<GameSearchDto>
    {
        public GameSearchValidator()
        {
            RuleFor(s => s.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(s => s.MinPrice.HasValue)
                .WithMessage("MinPrice must be >= 0");

            RuleFor(s => s.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(s => s.MaxPrice.HasValue)
                .WithMessage("MaxPrice must be >= 0");

            RuleFor(s => s.Page)
                .GreaterThan(0)
                .WithMessage("Page must be at least 1");

            RuleFor(s => s.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");

            RuleFor(s => s.Sort)
                .Must(sort => sort == null || new[] { "price", "price_desc", "name", "name_desc", "genre", "genre_desc" }
                    .Contains(sort.ToLower()))
                .WithMessage("Sort must be one of: price, price_desc, name, name_desc, genre, genre_desc");
        }
    }
}
