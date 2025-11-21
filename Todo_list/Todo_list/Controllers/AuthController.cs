// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Todo_list.DTO;
using Todo_list.Services;

namespace Todo_list.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { error = "Invalid input data" });

                var result = await _authService.RegisterAsync(registerDto);

                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);

                return CreatedAtAction(nameof(Register), new
                {
                    userId = result.Username
                }, new
                {
                    userId = result.Username,
                    message = "User registered successfully",
                    token = result.Token,
                    username = result.Username,
                    email = result.Email,
                    role = result.Role,
                    expiresAt = result.ExpiresAt
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Registration failed (validation): {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Детальное логирование ошибки
                _logger.LogError(ex, "Error during registration for email: {Email}. Inner exception: {InnerException}",
                    registerDto.Email, ex.InnerException?.Message);
                return StatusCode(500, new { error = $"Registration failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <returns>JWT токен и информация о пользователе</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { error = "Invalid input data" });

                var result = await _authService.LoginAsync(loginDto);

                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);

                return Ok(new
                {
                    token = result.Token,
                    username = result.Username,
                    email = result.Email,
                    role = result.Role,
                    expiresAt = result.ExpiresAt,
                    message = "Login successful"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Detailed error during login for email: {Email}. Stack trace: {StackTrace}",
                    loginDto.Email, ex.StackTrace);
                return StatusCode(500, new { error = "Internal server error during login" });
            }
        }

        /// <summary>
        /// Проверка валидности токена (опционально)
        /// </summary>
        /// <returns>Информация о текущем пользователе</returns>
        [HttpPost("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                username,
                email,
                role,
                message = "Token is valid"
            });
        }
    }
}