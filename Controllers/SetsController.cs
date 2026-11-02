using melee_tracker_capstone.DTOs;
using melee_tracker_capstone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace melee_tracker_capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly SetService _setService;

        public SetsController(SetService setService)
        {
            _setService = setService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(CreateSetRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

            var id = await _setService.CreateManualSetAsync(userId, request);

            return Ok(id);
        }
    }
}
