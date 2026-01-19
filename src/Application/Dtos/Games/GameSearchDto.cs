using Domain.Entities;

namespace Application.Dtos.Games
{
    public class GameSearchDto
    {
        public string? Term { get; set; }
        public Genre? Genre { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public AgeRating? AgeRating { get; set; }
        public string[]? Tags { get; set; }
        public string? Sort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
