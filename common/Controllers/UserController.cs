using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace common.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize] // ⚡ Yêu cầu xác thực JWT
        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            var username = User.Identity.Name;
            return Ok(new { Message = $"Hello {username}, this is your profile." });
        }
    }
}

