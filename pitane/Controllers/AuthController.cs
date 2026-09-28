using Microsoft.AspNetCore.Mvc;
using pitaneAPI.Models;

namespace pitaneAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegistrationDto registrationDto)
        {
            return Ok(ApiResponse<UserDto>.Ok(null, "User registered successfully"));
        }
    }
}
