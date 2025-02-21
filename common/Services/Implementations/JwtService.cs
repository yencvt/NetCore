using System;
using System.Security.Claims;
using System.Security.Cryptography;
using common.Services.Interfaces;

namespace common.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpiration;
        private readonly int _refreshTokenExpiration;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _secret = _config["JwtSettings:Secret"];
            _issuer = _config["JwtSettings:Issuer"];
            _audience = _config["JwtSettings:Audience"];
            _accessTokenExpiration = int.Parse(_config["JwtSettings:AccessTokenExpirationMinutes"]);
            _refreshTokenExpiration = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"]);
        }

        // ✅ Tạo Access Token
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ Tạo Refresh Token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

