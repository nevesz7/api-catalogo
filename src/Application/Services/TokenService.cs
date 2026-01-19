using Microsoft.IdentityModel.Tokens;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Services
{
    public class TokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly string _jwtKey;

        public TokenService(UserManager<User> userManager, string jwtKey)
        {
            _userManager = userManager;
            _jwtKey = jwtKey;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Viewer";

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, role),
                new Claim("birthDate", user.BirthDate.ToString("yyyy-MM-dd")),
                new Claim("loginTimestamp", DateTime.UtcNow.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
