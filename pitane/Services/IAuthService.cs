
using Pitane.DTO;

namespace pitaneAPI.Services
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> IsEmailExistAsync(string email);
    }
}
