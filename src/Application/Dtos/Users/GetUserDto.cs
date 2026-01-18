using System;
using System.ComponentModel.DataAnnotations;

namespace api_catalogo.Data.Dtos
{
    public class GetUserDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
