using System;

namespace Application.Dtos.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public string RePassword { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
