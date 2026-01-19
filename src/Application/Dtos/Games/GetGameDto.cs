using Domain.Entities;
using System.Collections.Generic;
using System;

namespace Application.Dtos.Games
{
    public class GetGameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public decimal Price { get; set; }
        public AgeRating AgeRating { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
