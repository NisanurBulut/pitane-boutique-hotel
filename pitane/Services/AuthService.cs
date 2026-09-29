using Microsoft.EntityFrameworkCore;
using AutoMapper;
using pitaneAPI.Data;
using pitaneAPI.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
namespace pitaneAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly PitaneDbContext _pitaneDbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(PitaneDbContext pitaneDbContext, IConfiguration configuration, IMapper mapper)
        {
            _pitaneDbContext = pitaneDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<UserDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto)
        {
            if (await IsEmailExistAsync(registerationRequestDto.Email))
            {
                return null;
            }

            User user = new User
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

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _pitaneDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDto.Email.ToLower() && u.Password == loginRequestDto.Password);

            if (user == null)
            {
                return null;
            }

            // generate JWT token

            return new LoginResponseDto
            {
                UserDto = _mapper.Map<UserDto>(user),
                Token = GenerateJwtToken(user)
            };
        }


        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _pitaneDbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        private string GenerateJwtToken(User user)
        {
            var pitaneKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSettings:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(pitaneKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
