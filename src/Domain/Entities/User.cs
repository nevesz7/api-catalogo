using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public override string? UserName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public User() : base() {}
    }
}
