using System;

namespace Application.Dtos.Users
{
    public class GetUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
