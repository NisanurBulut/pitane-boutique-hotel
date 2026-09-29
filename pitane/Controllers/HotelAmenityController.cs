using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitaneAPI.Data;
using pitaneAPI.Models;
using AutoMapper;

namespace pitaneAPI.Controllers
{
    [ApiController]
    [Route("api/hotel-amenity")]
    public class HotelAmenityController : ControllerBase
    {
        private readonly PitaneDbContext _pitaneDbContext;
        private readonly IMapper _mapper;
        public HotelAmenityController(PitaneDbContext pitaneDbContext, IMapper mapper)
        {
            _pitaneDbContext = pitaneDbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelAmenityDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<HotelAmenityDto>>>> GetHotelAmenities()
        {
            var hotels = await _pitaneDbContext.HotelAmenities.ToListAsync();
            var HotelAmenityDtos = _mapper.Map<IEnumerable<HotelAmenityDto>>(hotels);
            var apiResponse = ApiResponse<IEnumerable<HotelAmenityDto>>.Ok(HotelAmenityDtos, "Hotel Amenities retrieved successfully.");
            return Ok(apiResponse);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<HotelAmenityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<HotelAmenityDto>>> GetHotelAmenityById(int id)
        {
            if(id <= 0)
            {
                return BadRequest(ApiResponse<HotelAmenityDto>.BadRequest("Invalid hotel Amenity ID.", new { Id = "Hotel Amenity ID must be a positive integer." }));
            }

            var hotel = await _pitaneDbContext.HotelAmenities.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return ApiResponse<HotelAmenityDto>.NotFound("Hotel amenity not found.");     
            }
            return Ok(ApiResponse<HotelAmenityDto>.Ok(_mapper.Map<HotelAmenityDto>(hotel), "Hotel amenity retrieved successfully."));
        }

        [HttpPost]

        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<HotelCreateDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<HotelCreateDto>>> CreateHotelAmenity(HotelCreateDto createHotelAmenityDto)
        {
            if (createHotelAmenityDto == null)
            {
                return BadRequest(ApiResponse<HotelCreateDto>.BadRequest("Invalid hotel amenity data.", new { CreateHotelAmenityDto = "Hotel amenity data is required." }));
            }

            var hotelAmenity = _mapper.Map<HotelAmenity>(createHotelAmenityDto);
            hotelAmenity.CreatedTime= DateTime.Now;

            _pitaneDbContext.HotelAmenities.Add(hotelAmenity);

            await _pitaneDbContext.SaveChangesAsync();
            var response = ApiResponse<HotelCreateDto>.CreatedAt("Hotel amenity created successfully.", _mapper.Map<HotelCreateDto>(hotelAmenity));
            return CreatedAtAction(nameof(GetHotelAmenities), new { id = hotelAmenity.Id }, response);
        }

        [HttpPut("{id}")]

        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelAmenityDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<HotelAmenityUpdateDto>>> UpdateHotelAmenity(int id, HotelAmenityUpdateDto updateHotelAmenityDto)
        {
            if (updateHotelAmenityDto == null || id != updateHotelAmenityDto.Id)
            {
                return BadRequest(ApiResponse<HotelAmenityUpdateDto>.BadRequest("Invalid hotel amenity data.", new { HotelAmenityUpdateDto = "Invalid hotel amenity data." }) );
            }

            var existingHotelAmenity = await _pitaneDbContext.HotelAmenities.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotelAmenity == null)
            {
                return NotFound(ApiResponse<HotelAmenityUpdateDto>.NotFound("Hotel amenity not found."));
            }
            var duplicateHotelAmenity = await _pitaneDbContext.HotelAmenities
                .FirstOrDefaultAsync(h => h.Name.ToLower() == updateHotelAmenityDto.Name.ToLower() && h.Id != id);
            if (duplicateHotelAmenity != null)
            {
                return Conflict(ApiResponse<HotelAmenityUpdateDto>.Conflict("A hotel amenity with the same name already exists."));
            }
            _mapper.Map(updateHotelAmenityDto, existingHotelAmenity);

            existingHotelAmenity.UpdatedTime = DateTime.Now;

            await _pitaneDbContext.SaveChangesAsync();

            return Ok(ApiResponse<HotelAmenityUpdateDto>.Ok(_mapper.Map<HotelAmenityUpdateDto>(existingHotelAmenity), "Hotel amenity updated successfully."));
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelAmenityDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteHotelAmenity(int id)
        {
            var existingHotelAmenity = await _pitaneDbContext.HotelAmenities.FirstOrDefaultAsync(h => h.Id == id);

            if (existingHotelAmenity == null)
            {
                return NotFound(ApiResponse<bool>.NotFound("Hotel amenity not found."));
            }

            _pitaneDbContext.HotelAmenities.Remove(existingHotelAmenity);
            await _pitaneDbContext.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Hotel amenity deleted successfully."));
        }
    }
}
