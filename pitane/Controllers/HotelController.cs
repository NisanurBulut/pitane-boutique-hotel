using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitaneAPI.Data;
using pitaneAPI.Model;
using AutoMapper;

namespace pitaneAPI.Controllers
{
    [ApiController]
    [Route("api/hotel")]
    public class HotelController : ControllerBase
    {
        private readonly PitaneDbContext _pitaneDbContext;
        private readonly IMapper _mapper;
        public HotelController(PitaneDbContext pitaneDbContext, IMapper mapper)
        {
            _pitaneDbContext = pitaneDbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _pitaneDbContext.Hotels.ToListAsync();
            return Ok(hotels);
        }
        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel(CreateHotelDto createHotelDto)
        {
            if (createHotelDto == null)
            {
                return BadRequest();
            }

            var hotel = _mapper.Map<Hotel>(createHotelDto);

            _pitaneDbContext.Hotels.Add(hotel);

            await _pitaneDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotels), new { id = hotel.Id }, hotel);
        }
    }
}
