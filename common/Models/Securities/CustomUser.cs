using System;
using System.Security.Claims;

namespace common.Models.Securities
{
    public class CustomUser
    {
        private readonly ClaimsPrincipal _principal;

        public CustomUser(ClaimsPrincipal principal)
        {
            _principal = principal;
        }

        public string UserId => _principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        public string Email => _principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        public string Role => _principal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        public bool IsAuthenticated => _principal.Identity?.IsAuthenticated ?? false;
    }

}

