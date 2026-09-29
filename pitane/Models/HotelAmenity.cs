using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pitaneAPI.Models
{
    public class HotelAmenity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime? UpdatedTime { get; set; }

        [Required]
        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }

    }
}
