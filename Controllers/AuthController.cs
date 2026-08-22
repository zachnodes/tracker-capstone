using Microsoft.AspNetCore.Mvc;
using melee_tracker_capstone.DTOs;
using melee_tracker_capstone.Services;
using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

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

            Response.Cookies.Append("auth_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = new DateTimeOffset(result.ExpiresAt, TimeSpan.Zero)
            });

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return Ok(new { message = "Logged out successfully"});
        }


        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            if (userId == null)
                return Unauthorized();

            return Ok(new { userId, email});
        }

    }
}
