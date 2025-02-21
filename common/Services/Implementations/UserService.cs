using System.Security.Claims;
using common.Models.Securities;
using common.Services.Interfaces;

namespace common.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CustomUser GetCurrentUser => new CustomUser(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal());

        string IUserService.GetCurrentUserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
    }
}

