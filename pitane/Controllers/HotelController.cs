using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitaneAPI.Data;
using pitaneAPI.Model;

namespace pitaneAPI.Controllers
{
    [ApiController]
    [Route("api/hotel")]
    public class HotelController : ControllerBase
    {
        private readonly PitaneDbContext _pitaneDbContext;
        public HotelController(PitaneDbContext pitaneDbContext)
        {
            _pitaneDbContext = pitaneDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _pitaneDbContext.Hotels.ToListAsync();
            return Ok(hotels);
        }
    }
}
