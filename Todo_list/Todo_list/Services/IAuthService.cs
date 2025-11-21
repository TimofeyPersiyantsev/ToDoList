using Todo_list.DTO;

namespace Todo_list.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
        string GenerateJwtToken(User user);
    }
}
