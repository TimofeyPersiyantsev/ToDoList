using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
using Microsoft.EntityFrameworkCore;

namespace Todo_list.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
        {
            try
            {
                // Проверяем, существует ли пользователь с таким email или username
               // if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
               //     throw new ArgumentException("User with this email already exists");

                //if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                  //  throw new ArgumentException("User with this username already exists");

                // Хешируем пароль
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Создаем пользователя
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };

                // Сохраняем в базу
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Генерируем токен
                var token = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}", ex);
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            try
            {
                // Ищем пользователя по email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                    throw new UnauthorizedAccessException("Invalid email or password");

                // Генерируем токен
                var token = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}", ex);
            }
        }

        public string GenerateJwtToken(User user)
        {
            try
            {
                // Используем фиксированный ключ для тестирования
                var keyString = "super_secret_key_that_is_very_long_and_secure_123!";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var token = new JwtSecurityToken(
                    issuer: "TodoListAPI",
                    audience: "TodoListApp",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"JWT token generation failed: {ex.Message}", ex);
            }
        }
    }
}