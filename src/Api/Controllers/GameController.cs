using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Dtos.Games;
using Application.Services;
using Application.Profiles;
using Infra;
using AutoMapper;

namespace Api_Catalogo.Api.Controllers
{
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        private readonly GameSearchService _searchService;
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public GameController(GameSearchService searchService, UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _searchService = searchService;
        }

        // GET games/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? term,
            [FromQuery] Genre? genre,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] AgeRating? ageRating,
            [FromQuery] string[]? tags,
            [FromQuery] string? sort,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _searchService.SearchAsync(term, genre, minPrice, maxPrice, ageRating, tags, sort, page, pageSize);
            return Ok(result);
        }

        // POST /games
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var game = _mapper.Map<Game>(dto);

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<GetGameDto>(game);
            return Ok(new { message = "Game created successfully", game = response });
        }

        // GET /games
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _context.Games.ToListAsync();
            var dtos = _mapper.Map<List<GetGameDto>>(games);
            return Ok(dtos);
        }

        // GET /games/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            var dto = _mapper.Map<GetGameDto>(game);
            return Ok(dto);
        }

        // PUT /games/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameDto dto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Name)) game.Name = dto.Name;
            if (dto.Genre.HasValue) game.Genre = dto.Genre.Value;
            if (dto.Price.HasValue) game.Price = dto.Price.Value;
            if (dto.AgeRating.HasValue) game.AgeRating = dto.AgeRating.Value;
            if (dto.Tags != null) game.Tags = dto.Tags;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Game updated successfully" });
        }
    }
}
