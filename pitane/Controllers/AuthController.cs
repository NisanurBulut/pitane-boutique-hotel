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
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterationRequestDto registrationDto)
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
    }
}
