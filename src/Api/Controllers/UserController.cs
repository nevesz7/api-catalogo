using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Dtos.Users;
using Domain.Entities;
using AutoMapper;

namespace Api_Catalogo.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // POST /users/auth/register
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleToAssign = string.IsNullOrWhiteSpace(dto.Role) ? Roles.Viewer : dto.Role;
            if (!await _roleManager.RoleExistsAsync(roleToAssign))
                return BadRequest(new { message = $"Role '{roleToAssign}' does not exist" });

            await _userManager.AddToRoleAsync(user, roleToAssign);

            return Ok(new { message = "User registered successfully" });
        }

        // POST /users/auth/login
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { message = "Login successful" });
        }

        // GET /users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await Task.FromResult(_userManager.Users.ToList());
            var dtos = new List<GetUserDto>();

            foreach (var user in users)
            {
                var dto = _mapper.Map<GetUserDto>(user);
                dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Viewer";
                dtos.Add(dto);
            }

            return Ok(dtos);
        }

        // GET /users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var dto = _mapper.Map<GetUserDto>(user);
            dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Viewer";

            return Ok(dto);
        }

        // PUT /users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (dto.BirthDate.HasValue)
                user.BirthDate = dto.BirthDate.Value;

            // Admin can update roles
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    return BadRequest(new { message = $"Role '{dto.Role}' does not exist" });

                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User updated successfully" });
        }

        // DELETE /users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
