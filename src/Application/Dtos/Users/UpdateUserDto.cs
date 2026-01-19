using System;

namespace Application.Dtos.Users
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Role { get; set; }
    }
}
