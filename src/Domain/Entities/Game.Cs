using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum Genre
    {
        Action,
        Adventure,
        RPG,
        Strategy,
        Simulation,
        Sports
    }

    public enum AgeRating
    {
        Age0 = 0,
        Age10 = 10,
        Age12 = 12,
        Age14 = 14,
        Age18 = 18
    }

    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public AgeRating AgeRating { get; set; }

        public List<string> Tags { get; set; } = new();

        public Game() { }
    }
}
