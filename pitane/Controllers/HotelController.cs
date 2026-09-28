using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitaneAPI.Data;
using pitaneAPI.Model;
using AutoMapper;
using pitaneAPI.Model.DTO;

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
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _pitaneDbContext.Hotels.ToListAsync();
            return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<HotelDto>>> GetHotelById(int id)
        {
            if(id <= 0)
            {
                return new ApiResponse<HotelDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid hotel ID.",
                    Success = false,
                    Errors = new { Id = "Hotel ID must be a positive integer." },
                };
            }

            var hotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return new ApiResponse<HotelDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Hotel not found.",
                    Success = false,
                };
            }
            return new ApiResponse<HotelDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Hotel retrieved successfully.",
                Success = true,
                Data = _mapper.Map<HotelDto>(hotel)
            };
        }

        [HttpPost]
        public async Task<ActionResult<CreateHotelDto>> CreateHotel(CreateHotelDto createHotelDto)
        {
            if (createHotelDto == null)
            {
                return BadRequest();
            }

            var hotel = _mapper.Map<Hotel>(createHotelDto);

            _pitaneDbContext.Hotels.Add(hotel);

            await _pitaneDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotels), new { id = hotel.Id }, _mapper.Map<CreateHotelDto>(hotel));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateHotelDto>> UpdateHotel(int id, UpdateHotelDto updateHotelDto)
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
            var duplicateHotel = await _pitaneDbContext.Hotels
                .FirstOrDefaultAsync(h => h.Name.ToLower() == updateHotelDto.Name.ToLower() && h.Id != id);
            if (duplicateHotel != null)
            {
                return Conflict("A hotel with the same name already exists.");
            }
            _mapper.Map(updateHotelDto, existingHotel);
            existingHotel.UpdatedTime = DateTime.Now;
            await _pitaneDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<UpdateHotelDto>(existingHotel));
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
