using System.ComponentModel.DataAnnotations;

namespace Pitane.DTO
{
    public class RegisterationRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Customer";
    }
}
