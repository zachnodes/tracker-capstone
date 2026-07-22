using Microsoft.AspNetCore.Mvc;
using melee_tracker_capstone.DTOs;
using melee_tracker_capstone.Services;
using Amazon.Runtime.Internal;

namespace melee_tracker_capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        { 
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email and password are required" });

            var result = await _authService.Register(request);

            if (result == null)
                return Conflict(new { message = "Account could not be created. Email already exists" });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest(new { message = "Email and password are required" });

            var result = await _authService.Login(login);

            if (result == null)
                // Unauthorized instead of NotFound for security concerns
                // Intentionally vague
                return Unauthorized(new { message = "Could not find account, invalid email or password" });

            return Ok(result);
        }
    }
}
