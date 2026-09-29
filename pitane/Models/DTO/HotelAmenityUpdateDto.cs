using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pitaneAPI.Models.DTO
{
    public class HotelAmenityUpdateDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;

        [Required]
        public int HotelId { get; set; }
    }
}
