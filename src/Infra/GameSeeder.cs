using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra
{
    public static class GameSeeder
    {
        public static void Seed(UserDbContext context)
        {
            if (!context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game { Name = "Epic Fantasy RPG", Genre = Genre.RPG, Price = 49.99m, AgeRating = AgeRating.Age12, Tags = new List<string>{"fantasy","multiplayer"} },
                    new Game { Name = "Mini RPG Adventure", Genre = Genre.RPG, Price = 29.99m, AgeRating = AgeRating.Age12, Tags = new List<string>{"fantasy"} },
                    new Game { Name = "Action Shooter 2026", Genre = Genre.Action, Price = 59.99m, AgeRating = AgeRating.Age18, Tags = new List<string>{"shooter","multiplayer"} },
                    new Game { Name = "Soccer Pro 2026", Genre = Genre.Sports, Price = 39.99m, AgeRating = AgeRating.Age10, Tags = new List<string>{"sports","multiplayer"} },
                    new Game { Name = "Strategy Master", Genre = Genre.Strategy, Price = 44.99m, AgeRating = AgeRating.Age14, Tags = new List<string>{"strategy","singleplayer"} }
                };

                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}
