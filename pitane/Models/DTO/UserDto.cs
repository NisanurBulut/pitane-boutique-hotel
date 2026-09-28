using System.ComponentModel.DataAnnotations;

namespace pitaneAPI.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Role { get; set; } = default!;    
    }
}
