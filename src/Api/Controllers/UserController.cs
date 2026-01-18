using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api_catalogo.Data.Dtos;
using api_catalogo.Models;

namespace api_catalogo.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // =========================
        // AUTH
        // =========================

        // POST /users/auth/register
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = dto.Username,
                BirthDate = dto.BirthDate
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

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

        // =========================
        // USERS CRUD
        // =========================

        // GET /users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users
                .Select(u => new GetUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    BirthDate = u.BirthDate
                })
                .ToList();

            return Ok(users);
        }

        // GET /users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var response = new GetUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                BirthDate = user.BirthDate
            };

            return Ok(response);
        }

        // PUT /users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (dto.BirthDate.HasValue)
                user.BirthDate = dto.BirthDate.Value;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User updated successfully" });
        }

        // DELETE /users/{id}
        [HttpDelete("{id}")]
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
