using Domain.Entities;
using System.Collections.Generic;

namespace Application.Dtos.Games
{
    public class GameSearchResultDto
    {
        public IEnumerable<GetGameDto> Games { get; set; } = new List<GetGameDto>();
        public int TotalCount { get; set; }
        public decimal AveragePrice { get; set; }

        public List<GetGameDto> Top3Expensive { get; set; } = new();
    }
}
