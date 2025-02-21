using System;
using common.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using common.Models.Securities;

namespace common.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        // ✅ Đăng nhập
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // ⚠ Giả lập kiểm tra tài khoản (Bạn nên dùng Database thật)
            if (request.Username == "admin" && request.Password == "password")
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

                var accessToken = _jwtService.GenerateAccessToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}

