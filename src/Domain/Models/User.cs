using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace api_catalogo.Models
{
    public class User : IdentityUser
    {
        [Required]
        public override string UserName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public User() : base() {}
    }
}
