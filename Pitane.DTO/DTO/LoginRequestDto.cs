using System.ComponentModel.DataAnnotations;

namespace Pitane.DTO
{
    public class LoginRequestDto
    {
        [Required]
        public required string Password { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
