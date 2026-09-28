using System.ComponentModel.DataAnnotations;

namespace pitaneAPI.Models
{
    public class UpdateHotelDto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public required string Name { get; set; }
        public string Details { get; set; } = default!;
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
