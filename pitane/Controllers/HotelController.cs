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
        public async Task<ActionResult<ApiResponse<IEnumerable<HotelDto>>>> GetHotels()
        {
            var hotels = await _pitaneDbContext.Hotels.ToListAsync();
            var hotelDtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);
            var apiResponse = ApiResponse<IEnumerable<HotelDto>>.Ok(hotelDtos, "Hotels retrieved successfully.");
            return Ok(apiResponse);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<HotelDto>>> GetHotelById(int id)
        {
            if(id <= 0)
            {
                return BadRequest(ApiResponse<HotelDto>.BadRequest("Invalid hotel ID.", new { Id = "Hotel ID must be a positive integer." }));
            }

            var hotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return ApiResponse<HotelDto>.NotFound("Hotel not found.");     
            }
            return Ok(ApiResponse<HotelDto>.Ok(_mapper.Map<HotelDto>(hotel), "Hotel retrieved successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateHotelDto>>> CreateHotel(CreateHotelDto createHotelDto)
        {
            if (createHotelDto == null)
            {
                return BadRequest(ApiResponse<CreateHotelDto>.BadRequest("Invalid hotel data.", new { CreateHotelDto = "Hotel data is required." }));
            }

            var hotel = _mapper.Map<Hotel>(createHotelDto);

            _pitaneDbContext.Hotels.Add(hotel);

            await _pitaneDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotels), new { id = hotel.Id }, ApiResponse<CreateHotelDto>.Ok(_mapper.Map<CreateHotelDto>(hotel), "Hotel created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UpdateHotelDto>>> UpdateHotel(int id, UpdateHotelDto updateHotelDto)
        {
            if (updateHotelDto == null || id != updateHotelDto.Id)
            {
                return BadRequest(ApiResponse<UpdateHotelDto>.BadRequest("Invalid hotel data.", new { UpdateHotelDto = "Invalid hotel data." }) );
            }

            var existingHotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotel == null)
            {
                return NotFound(ApiResponse<UpdateHotelDto>.NotFound("Hotel not found."));
            }
            var duplicateHotel = await _pitaneDbContext.Hotels
                .FirstOrDefaultAsync(h => h.Name.ToLower() == updateHotelDto.Name.ToLower() && h.Id != id);
            if (duplicateHotel != null)
            {
                return Conflict(ApiResponse<UpdateHotelDto>.Conflict("A hotel with the same name already exists."));
            }
            _mapper.Map(updateHotelDto, existingHotel);
            existingHotel.UpdatedTime = DateTime.Now;
            await _pitaneDbContext.SaveChangesAsync();

            return Ok(ApiResponse<UpdateHotelDto>.Ok(_mapper.Map<UpdateHotelDto>(existingHotel), "Hotel updated successfully."));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteHotel(int id)
        {
            var existingHotel = await _pitaneDbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotel == null)
            {
                return NotFound(ApiResponse<bool>.NotFound("Hotel not found."));
            }

            _pitaneDbContext.Hotels.Remove(existingHotel);
            await _pitaneDbContext.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Hotel deleted successfully."));
        }
    }
}
