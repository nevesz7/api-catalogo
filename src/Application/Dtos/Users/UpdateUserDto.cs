using System;
using System.ComponentModel.DataAnnotations;

namespace api_catalogo.Data.Dtos
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
