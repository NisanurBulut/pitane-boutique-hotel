using System.ComponentModel.DataAnnotations;

namespace pitaneAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
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
        public required string Rol { get; set; } = "Customer";
        public DateTime CreatedTime { get; set; } 
        public DateTime? UpdatedTime { get; set; } 
    }
}
