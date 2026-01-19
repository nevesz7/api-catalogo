using Application.Dtos.Games;
using Domain.Entities;
using Infra;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class GameSearchService
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public GameSearchService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GameSearchResultDto> SearchAsync(
            string? term,
            Genre? genre,
            decimal? minPrice,
            decimal? maxPrice,
            AgeRating? ageRating,
            string[]? tags,
            string? sort,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Games.AsQueryable();

            // --- Filters ---
            if (!string.IsNullOrEmpty(term)) query = query.Where(g => g.Name.Contains(term));
            if (genre.HasValue) query = query.Where(g => g.Genre == genre.Value);
            if (ageRating.HasValue) query = query.Where(g => g.AgeRating == ageRating.Value);
            if (minPrice.HasValue) query = query.Where(g => g.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(g => g.Price <= maxPrice.Value);
            if (tags != null && tags.Any()) query = query.Where(g => g.Tags.Any(t => tags.Contains(t)));

            // --- Aggregations ---
            var totalCount = await query.CountAsync();
            var averagePrice = totalCount > 0 ? await query.AverageAsync(g => g.Price) : 0;

            // --- Sorting ---
            query = sort?.ToLower() switch
            {
                "price" => query.OrderBy(g => g.Price),
                "price_desc" => query.OrderByDescending(g => g.Price),
                "name" => query.OrderBy(g => g.Name),
                "name_desc" => query.OrderByDescending(g => g.Name),
                "genre" => query.OrderBy(g => g.Genre),
                "genre_desc" => query.OrderByDescending(g => g.Genre),
                _ => query.OrderBy(g => g.Name)
            };

            // --- Pagination ---
            var gamesPaged = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var gamesDto = _mapper.Map<List<GetGameDto>>(gamesPaged);

            // --- Top 3 expensive ---
            var top3Expensive = await query
                .OrderByDescending(g => g.Price)
                .Take(3)
                .ToListAsync();

            var top3Dto = _mapper.Map<List<GetGameDto>>(top3Expensive);

            // --- Return result DTO ---
            return new GameSearchResultDto
            {
                Games = gamesDto,
                TotalCount = totalCount,
                AveragePrice = averagePrice,
                Top3Expensive = top3Dto
            };
        }
    }
}
