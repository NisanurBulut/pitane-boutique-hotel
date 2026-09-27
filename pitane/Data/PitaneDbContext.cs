using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using pitaneAPI.Model;

namespace pitaneAPI.Data
{
    public class PitaneDbContext : DbContext
    {
        public PitaneDbContext(DbContextOptions<PitaneDbContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
    }
}