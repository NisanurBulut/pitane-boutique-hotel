using Microsoft.EntityFrameworkCore;
using AutoMapper;
using pitaneAPI.Data;
using pitaneAPI.Models;
namespace pitaneAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly PitaneDbContext _pitaneDbContext;
        private readonly IMapper _mapper;
        public AuthService(PitaneDbContext pitaneDbContext,IConfiguration configuration, IMapper mapper)
        {
            _pitaneDbContext = pitaneDbContext;
            _mapper = mapper;
        }
        public async Task<UserDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto)
        {
            if(await IsEmailExistAsync(registerationRequestDto.Email))
            {
                return null;
            }

            User user= new User
            {
                Email = registerationRequestDto.Email,
                Name = registerationRequestDto.Name,
                Password = registerationRequestDto.Password, 
                Rol = string.IsNullOrEmpty(registerationRequestDto.Role) ? "Customer" : registerationRequestDto.Role,
                CreatedTime = DateTime.UtcNow
            };
            _pitaneDbContext.Users.Add(user);
            await _pitaneDbContext.SaveChangesAsync();
            // Implement registration logic here
            return _mapper.Map<UserDto>(user);
        }

        public Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            // Implement login logic here
            return Task.FromResult<LoginResponseDto?>(null);
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _pitaneDbContext.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
