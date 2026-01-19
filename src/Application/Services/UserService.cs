using Application.Dtos.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Application.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<bool> RoleExistsAsync(string role) =>
            await _roleManager.RoleExistsAsync(role);

        public async Task<User> RegisterAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            var roleToAssign = string.IsNullOrWhiteSpace(dto.Role) ? Roles.Viewer : dto.Role;

            if (!await _roleManager.RoleExistsAsync(roleToAssign))
                throw new InvalidOperationException($"Role '{roleToAssign}' does not exist");

            await _userManager.AddToRoleAsync(user, roleToAssign);

            return user;
        }

        public async Task<List<GetUserDto>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var dtos = new List<GetUserDto>();

            foreach (var user in users)
            {
                var dto = _mapper.Map<GetUserDto>(user);
                dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Viewer";
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<GetUserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var dto = _mapper.Map<GetUserDto>(user);
            dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Viewer";
            return dto;
        }

        public async Task UpdateAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (dto.BirthDate.HasValue)
                user.BirthDate = dto.BirthDate.Value;

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    throw new InvalidOperationException($"Role '{dto.Role}' does not exist");

                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }
    }
}
