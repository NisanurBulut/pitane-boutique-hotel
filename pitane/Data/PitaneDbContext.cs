using Microsoft.EntityFrameworkCore;

namespace pitaneAPI.Data
{
    public class PitaneDbContext:DbContext
    {
        public PitaneDbContext(DbContextOptions<PitaneDbContext> options) : base(options)
        {
        }
    }
}
