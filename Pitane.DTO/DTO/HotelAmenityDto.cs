using System.ComponentModel.DataAnnotations;

namespace Pitane.DTO
{
    public class HotelAmenityDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;

        [Required]
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
    }
}
