using melee_tracker_capstone.DTOs;
using melee_tracker_capstone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace melee_tracker_capstone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReplayController : ControllerBase
    {
        private readonly ReplayService _replayService;

        public ReplayController(ReplayService replayService) 
        { 
            _replayService = replayService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] UploadReplayRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);
            var result = await _replayService.UploadReplay(request.File, userId);

            return Accepted(result);


        }


    }
}
