

using System.ComponentModel.DataAnnotations;

namespace pitaneAPI.Models.DTO
{
    public class HotelAmenityCreateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int HotelId { get; set; }
    }
}
