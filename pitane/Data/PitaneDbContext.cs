using Microsoft.EntityFrameworkCore;
using pitaneAPI.Models;

namespace pitaneAPI.Data
{
    public class PitaneDbContext : DbContext
    {
        public PitaneDbContext(DbContextOptions<PitaneDbContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
    }
}