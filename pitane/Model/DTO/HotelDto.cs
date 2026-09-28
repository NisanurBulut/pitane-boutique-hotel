using System.ComponentModel.DataAnnotations;

namespace pitaneAPI.Model
{
    public class HotelDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Details { get; set; } = default!;
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}
