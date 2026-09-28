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

        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> UpdateHotel(int id, UpdateHotelDto updateHotelDto)
        {
            if (updateHotelDto == null || id != updateHotelDto.Id)
            {
                return BadRequest();
            }

            var existingHotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotel == null)
            {
                return NotFound();
            }

            _mapper.Map(updateHotelDto, existingHotel);
            existingHotel.UpdatedTime = DateTime.Now;
            await _pitaneDbContext.SaveChangesAsync();

            return Ok(existingHotel);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            var existingHotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotel == null)
            {
                return NotFound();
            }

            _pitaneDbContext.Hotels.Remove(existingHotel);
            await _pitaneDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
