using Application.Dtos.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Services;

namespace Api.Services
{
    public class UserAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;

        public UserAuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<string?> RegisterAsync(CreateUserDto dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                BirthDate = dto.BirthDate
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            var role = string.IsNullOrWhiteSpace(dto.Role) ? "Viewer" : dto.Role;

            if (!await _userManager.IsInRoleAsync(user, role))
                await _userManager.AddToRoleAsync(user, role);

            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task<string?> LoginAsync(LoginRequestDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (!result.Succeeded)
                return null;

            var userEntity = await _userManager.FindByNameAsync(dto.Username);
            if (userEntity == null) return null;

            return await _tokenService.GenerateTokenAsync(userEntity);
        }
    }
}
