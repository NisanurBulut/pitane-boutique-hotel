using Microsoft.AspNetCore.Mvc;
using pitaneAPI.Models;
using pitaneAPI.Services;

namespace pitaneAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterationRequestDto registrationDto)
        {
            if(registrationDto == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));
            }

            if(await _authService.IsEmailExistAsync(registrationDto.Email))
            {
                return Conflict(ApiResponse<object>.Conflict("Email already exists"));
            }

            var user = await _authService.RegisterAsync(registrationDto);

            if(user == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("User registration failed"));
            }
            var response = ApiResponse<UserDto>.CreatedAt("User registered successfully", user);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));
            }
            
            var loginResponse = await _authService.LoginAsync(loginDto);

            if (loginResponse == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Login failed"));
            }
            var response = ApiResponse<LoginResponseDto>.Ok(loginResponse, "Login successful");
            return Ok(response);
        }
    }
}