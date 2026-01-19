using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext (DbContextOptions<UserDbContext> opts) : base(opts)
        {
            
        }

        public DbSet<Game> Games { get; set; } = null!;
    }
}