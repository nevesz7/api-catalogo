using Application.Dtos.Users;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_Catalogo.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _userService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var dto = await _userService.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                await _userService.UpdateAsync(id, dto);
                return Ok(new { message = "User updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
