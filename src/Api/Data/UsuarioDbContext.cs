using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using api_catalogo.Models;

namespace api_catalogo.Data
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext (DbContextOptions<UserDbContext> opts) : base(opts)
        {
            
        }
    }
}