namespace pitaneAPI.Models
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public UserDto? UserDto { get; set; }
    }
}
